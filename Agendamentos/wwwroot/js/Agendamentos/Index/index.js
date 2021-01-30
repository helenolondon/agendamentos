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
        editable: true,
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

    $("#teste").on("click", function () {
        alert("teset");
    })

    calendar.render();
    //calendar.next();

    //calendar.scrollToTime('16:00')

    function addAgenmento() {
        var pop = $('#md-editar-agendamento');

        if (pop) {
            loadLookups(() => {
                onNovoAgendamento();
                pop.draggable();
                pop.modal();
            });
        }
    }

    function onNovoAgendamento() {
        clearAgendamentoForm();
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

    $([]).add("#sel-servico")
        .add("#txt-ag-inicio")
        .add("#txt-ag-termino")
        .add("#txt-data")
        .on('change', (e, b) => {
        loadProfissionais();
    });

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
        let codServico = parseInt($("#sel-servico").val());
        let codProfissional = parseInt($("#sel-profissional").val());
        let horaInicio = $("#txt-ag-inicio").val();
        let horaTermino = $("#txt-ag-termino").val();
        let data = $("#txt-data").val();
        let codAgendamentoItem = 0;

        if (Number.isNaN(codAgendamentoItem)) {
            codAgendamentoItem = 0;
        }

        amat.push({
            "CodAgendamentoItem": 0,
            "CodAgendamento": codAgendamento,
            "Inicio": data + "T" + horaInicio,
            "Termino": data + "T" + horaTermino,
            "CodServico": codServico,
            "CodProfissional": codProfissional
        });

        return amat;
    }

    function clearAgendamentoForm() {
        $("#cod-agendamento").val(0);
        $("#sel-servico").val(null);
        $("#txt-ag-inicio").val(null);
        $("#txt-ag-termino").val(null);
        $("#txt-data").val(null);
        $("#sel-cliente").val(null);
        $("#sel-status").val(1);
        $("#sel-profissional").val(null);
    }

    function onSchedulerRefreshNeeded(){
        calendar.refetchEvents();
    }

    function onEditarAgendamento(e) {

        var pop = $('#md-editar-agendamento');

        if (pop) {
            loadLookups(() => {

                $("#cod-agendamento").val(e.codAgendamento);
                $("#sel-servico").val(e.codServico);
                $("#txt-ag-inicio").val(e.horaInicio);
                $("#txt-data")[0].valueAsDate = new Date(e.dataAgendamento);
                $("#txt-ag-termino").val(e.horaTermino);
                $("#sel-cliente").val(e.codCliente);
                $("#sel-status").val(e.codStatus)

                loadProfissionais();
                $("#sel-profissional").val(e.codProfissional);

                pop.draggable();
                pop.modal();
            });
        };
    }

    // Carrega os profiossionais disponíveispara o agendamento
    function loadProfissionais(){

        let codServico = $("#sel-servico").val();
        let horaInicio = $("#txt-ag-inicio").val();
        let horaTermino = $("#txt-ag-termino").val();
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
            var select = $("#sel-profissional");
            select.find("option").remove();

            $.each(data, (i, item) => {
                select.append($("<option>", { value: item.codPessoa, text: item.nomePessoa }));
            });
        });
    }

    // Carrega os procedimetos do profiossinal
    function loadServicos() {
        var select = $("#sel-servico");

        select.find("option").remove();
        

        return $.get("api/servicos", (data) => {
            $.each(data, (i, item) => {
                var opt = $("<option>", { value: item.codServico, text: item.nomeServico });

                select.append(opt);
            });

            select.trigger('change');
        });
    }

    // Carrega Lookups
    function loadLookups(callBack) {

        var q1 = $.get("api/pessoas", (data) => {

            var select = $("#sel-cliente");
            select.find("option").remove();

            $.each(data, (i, item) => {
                select.append($("<option>", { value: item.codPessoa, text: item.nomePessoa }));
            })            
        })

        $.when(q1, loadServicos()).then(() => {
            callBack();
        })
    }
});


