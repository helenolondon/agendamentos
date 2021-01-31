document.addEventListener('DOMContentLoaded', function () {
    $.ajaxSetup({ contentType: "application/json; charset=utf-8" });

    var dialog, form;

    var calendarEl = document.getElementById('calendar');
    var calendar = new FullCalendar.Calendar(calendarEl, {
        initialView: 'timeGridWeek',
        timeZone: 'America/Sao_Paulo',
        locale: 'pt-br',
        weekends: true,
        slotDuration: '00:15:00',
        slotLabelInterval: '00:30',
        scrollTime: '08:00:00',
        editable: false,
        events: 'api/agendamentos',
        slotLabelFormat: {
            hour: 'numeric',
            minute: '2-digit',
            omitZeroMinute: false,
            meridiem: 'short'
        },
        customButtons: {
            novoAgendamento: {
                text: 'Agendar',
                click: addAgenmento
            }
        },
        headerToolbar: {
            left: 'prev,next today',
            center: 'title',
            right: 'novoAgendamento dayGridMonth,timeGridWeek'
        },
        eventContent: function (arg) {
            let span = document.createElement("span");
            span.innerHTML = "X";
            span.style = "float: right; margin-right: 6px; font-size: 10px"
            span.id = "teste";
            span.onclick = ((ev, e) => {
                onRemoverAgendamento(arg.event.extendedProps.codAgendamentoItem);
            })

            let ElHorario = document.createElement('p')

            ElHorario.innerHTML = arg.event.extendedProps.horarioLabel;
            ElHorario.style = "margin: 10px 2px 2px; font-size: 11px;"

            let ElCliente = document.createElement('p')

            ElCliente.innerHTML = arg.event.extendedProps.nomeCliente;
            ElCliente.style = "margin: 0px 2px 2px; font-size: 11px; margin-top: 10px;"

            let ElProfissional = document.createElement('p')
            ElProfissional.style = "font-size: 10px; margin: 0px 2px 2px;"

            ElProfissional.innerHTML = arg.event.extendedProps.nomeProfissional;

            let ElServico = document.createElement('p')
            ElServico.style = "font-size: 10px; margin: 0px 2px 2px;"

            ElServico.innerHTML = arg.event.extendedProps.servico;

            let ElCancelado = document.createElement('p')
            ElCancelado.style = "font-size: 10px; margin: 0px 2px 2px;"

            ElCancelado.innerHTML = arg.event.extendedProps.status;

            let arrayOfDomNodes = [span, ElCancelado, ElHorario, ElCliente, ElProfissional, ElServico]
            return { domNodes: arrayOfDomNodes }
        },
        eventClick: function (e) {
            if (e.jsEvent.layerY > 10) {
                onEditarAgendamento(e.event.extendedProps);
            }
        }
    });

    calendar.render();

    // Mostra a área emm volta do horári atual;
    calendar.scrollToTime(moment(new Date().getTime()).format("hh:mm"));

    function addAgenmento() {
        var pop = $('#md-editar-agendamento');

        if (pop) {
            loadClientes().then(() => {
                onNovoAgendamento();
                setPopAgendamentoTitle("Criar Agendamento");
                pop.draggable();
                pop.modal();
            });
        }
    }

    function setPopAgendamentoTitle(titulo) {
        $("#md-editar-agendamento")
            .find(".modal-title")
            .html(titulo);
    }

    function onNovoAgendamento() {
        clearAgendamentoForm();
        addProcedimentoItem();

        $("#txt-data")[0].valueAsDate = new Date();
    }

    form = $("form").submit(function (event) {
        event.preventDefault();

        onAgendamentoSalvar()
            .then(() => {
                $('#md-editar-agendamento').modal("hide");
            })
    });

    $("#btn-salvar").on("click", () => {
        form.submit();
    })

    $("#btn-cancelar").on('click', () => {
        form[0].reset();
    })

    $("#btn-novo-procedimento").click(() => {
        addProcedimentoItem();
    })

    $("#txt-data").on('change', function () {
        $(".procedimento-item")
            .find("input[type='time']")
            .first()
            .trigger('change');
    })

    function addProcedimentoItem() {
        const procedimentoTemplate = ({ id, codProcedimentoItem }) => retProcedimentoTemplate(id, codProcedimentoItem);
        let novaId = $(".procedimento-item").length + 1;

        $('#novo-procedimento').append([
            { id: novaId, codProcedimentoItem: '0' }

        ].map(procedimentoTemplate).join(''));

        $(".btn-remover-item").click(function () {
            if ($(".procedimento-item").length > 1) {
                $(this).parent().parent().remove();
            }

            botoesRemoverDisableEnable();
        });

        loadServicos(novaId);

        $([]).add("#sel-servico-" + novaId)
            .add("#txt-ag-inicio-" + novaId)
            .add("#txt-ag-termino-" + novaId)
            .on('change', (e, b) => {
                loadProfissionais(novaId);
            });

        botoesRemoverDisableEnable();

        return novaId;
    }

    function botoesRemoverDisableEnable() {
        if ($(".procedimento-item").length == 1) {
            $(".btn-remover-item").prop('disabled', true);
            $("#txt-data").prop('disabled', false);
        }
        else {
            $(".btn-remover-item").prop('disabled', false);
            $("#txt-data").prop('disabled', true);
        }
    }

    function onRemoverAgendamento(codAgendamentoItem) {

        var md = $("#md-confirma");
        md.find(".modal-body").html("<span>Confirma a exclusão do procedimento?</span>");
        md.draggable();
        md.find("#btn-sim").off('click');
        md.find("#btn-sim").on('click', () => {
            var q = $.ajax({
                url: 'api/agendamentos/remover?codAgendamentoItem=' + codAgendamentoItem,
                type: 'DELETE',
                success: function (result) {
                    onSchedulerRefreshNeeded()
                }
            });

            $.when([q]).then(() => {
                md.modal('hide');
            })
        })

        md.modal();
    }

    function onAgendamentoSalvar() {

        let codAgendamento = parseInt($("#cod-agendamento").val());
        let data = $("#txt-data").val();
        let codStatus = parseInt($("#sel-status").val());
        let codCliente = parseInt($("#sel-cliente").val());

        if (Number.isNaN(codAgendamento)) {
            codAgendamento = 0;
        }

        let request = {
            "CodAgendamento": codAgendamento,
            "Data": data,
            "CodStatus": codStatus,
            "CodCliente": codCliente,

            "Itens": retAgendamentoItens(codAgendamento)
        }

        return $.post("api/agendamentos/salvar", JSON.stringify(request), function () {
            clearAgendamentoForm();
            onSchedulerRefreshNeeded();
        });
    }

    function retAgendamentoItens(codAgendamento) {
        let amat = [];

        $(".procedimento-item").each(function (i) {
            let id = $(this).attr("data-id");

            let codServico = parseInt($("#sel-servico-" + id).val());
            let codProfissional = parseInt($("#sel-profissional-" + id).val());
            let horaInicio = $("#txt-ag-inicio-" + id).val();
            let horaTermino = $("#txt-ag-termino-" + id).val();
            let data = $("#txt-data").val();
            let codAgendamentoItem = parseInt($(this).attr("data-cod-procedimento"));

            if (Number.isNaN(codAgendamentoItem)) {
                codAgendamentoItem = 0;
            }

            amat.push({
                "CodAgendamentoItem": codAgendamentoItem,
                "CodAgendamento": codAgendamento,
                "Inicio": data + "T" + horaInicio,
                "Termino": data + "T" + horaTermino,
                "CodServico": codServico,
                "CodProfissional": codProfissional
            });
        })

        return amat;
    }

    function clearAgendamentoForm() {
        $("#cod-agendamento").val(0);
        $("#txt-data").val(null);
        $("#sel-cliente").val(null);
        $("#sel-status").val(1);

        $(".procedimento-item").remove();
    }

    function onSchedulerRefreshNeeded() {
        calendar.refetchEvents();
    }

    function onEditarAgendamento(e) {

        var pop = $('#md-editar-agendamento');
        var agendamento = consultarAgendamento(e.codAgendamento)
            .then((data) => {
                agendamento = data
            });

        setPopAgendamentoTitle("Editar Agendamento");
        clearAgendamentoForm();

        $.when(agendamento, loadClientes())
            .then(() => {

                // Cabeçalho
                $("#cod-agendamento").val(e.codAgendamento);
                $("#sel-cliente").val(e.codCliente);
                $("#txt-data")[0].valueAsDate = new Date(e.dataAgendamento);
                $("#sel-status").val(e.codStatus)

                $.each(agendamento.itens, (index, item) => {
                    var novoItem = addProcedimentoItem();

                    $(".procedimento-item[data-id=" + novoItem + "]").attr("data-cod-procedimento", item.codAgendamentoItem);
                    $("#sel-profissional-" + novoItem).val(item.codProfissional);
                    $("#sel-servico-" + novoItem).val(item.codServico);
                    $("#txt-ag-inicio-" + novoItem).val(item.horaInicio);
                    $("#txt-ag-termino-" + novoItem).val(item.horaTermino);
                });

                pop.draggable();
                pop.modal();
            });
    }

    function consultarAgendamento(codAgendamento) {
        return $.get("api/agendamentos/" + codAgendamento);
    }

    // Carrega os profiossionais disponíveispara o agendamento
    function loadProfissionais(id){

        if (Number.isNaN(id) || id == null) {
            return;
        }

        let codServico = $("#sel-servico-" + id).val();
        let horaInicio = $("#txt-ag-inicio-" + id).val();
        let horaTermino = $("#txt-ag-termino-" + id).val();
        let data = $("#txt-data").val();

        if (horaInicio.length == 0 || horaTermino.length == 0 || codServico == 0) {
            return;
        }

        //// Validação

        let request = {
            "Data": data,
            "HoraInicio": horaInicio,
            "HoraTermino": horaTermino,
            "CodServico": codServico
        }

        $.get("api/procedimentos/listar-profissionais-agendamento", request, (data) => {
            var select = $("#sel-profissional-" + id);
            select.find("option").remove();

            $.each(data, (i, item) => {
                select.append($("<option>", { value: item.codPessoa, text: item.nomePessoa }));
            });
        });
    }

    // Carrega os procedimetos do profiossinal
    function loadServicos(id) {
        var select = $("#sel-servico-" + id);

        select.find("option").remove();
        

        return $.get("api/servicos", (data) => {
            $.each(data, (i, item) => {
                var opt = $("<option>", { value: item.codServico, text: item.nomeServico });

                select.append(opt);
            });

            select.trigger('change');
        });
    }

    // Carrega clientes
    function loadClientes() {

        return $.get("api/pessoas", (data) => {

            var select = $("#sel-cliente");
            select.find("option").remove();

            $.each(data, (i, item) => {
                select.append($("<option>", { value: item.codPessoa, text: item.nomePessoa }));
            })            
        })
    }

    function retProcedimentoTemplate(id, codProcedimentoItem) {
        return `
<div class="procedimento-item border-top-10" data-id=${id} data-cod-procedimento=${codProcedimentoItem}>
    <div>Procedimento ${id} <button class="btn btn-link btn-remover-item" type="button" style="float: right">Remover</button></div>
    <hr />
    <!--Hora de início e fim-->
    <div class="container">
        <div class="row .no-gutters">
            <div class="col">
                <div class="form-group">
                    <label for=txt-ag-inicio-${id}>Início:</label>
                    <input type="time" class="form-control" id=txt-ag-inicio-${id}>
                </div>
            </div>
            <div class="col">
                <div class="form-group">
                    <label for=txt-ag-termino-${id}>Término:</label>
                    <input type="time" class="form-control" id=txt-ag-termino-${id}>
                </div>
            </div>
        </div>
    </div>

    <!--Procedimento-->
    <div class="form-group">
        <label for=sel-servico-${id}>Procedimento:</label>
        <select id=sel-servico-${id} class="custom-select">
            <option selected></option>
        </select>
    </div>

    <!--Profissional-->
    <div class="form-group no-border-bottom">
        <label for=sel-profissional-${id}>Profissional:</label>
        <select id=sel-profissional-${id} class="custom-select profissional">
            <option selected></option>
        </select>
    </div>
</div>
    `;
    }
});