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
            span.style = "float: right; margin-right: 6px;"
            span.onclick = ((ev, e) => {
                onRemoverAgendamento(arg.event.extendedProps.codAgendamento);
            })
            let El = document.createElement('p')

            El.innerHTML = arg.event.extendedProps.nomeCliente + "&nbsp; <br>" +
                arg.event.extendedProps.servico;
            El.style = "margin-top: 10px;"

            let arrayOfDomNodes = [span, El]
            return { domNodes: arrayOfDomNodes }
        }
    });

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

    $("#sel-servico").on('change', (e, b) => {
        //var selected = $("#sel-servico").find('option:selected');
        //var termino = selected.attr('data-horaTermino');

        //$("#txt-ag-inicio").val(inicio);
        //$("#txt-ag-termino").val(termino);
    });

    function onRemoverAgendamento(codAgendamento) {

        var md = $("#md-confirma");
        md.find(".modal-body").html("<span>Confirma a exclusão do agendamento?</span>");
        md.draggable();
        md.find("#btn-sim").off('click');
        md.find("#btn-sim").on('click', () => {
            var q = $.ajax({
                url: 'api/agendamentos/remover?codAgendamento=' + codAgendamento,
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

        var codAgendamento = parseInt($("#cod-agendamento").val());
        var codProcedimento = parseInt($("#sel-servico").val());
        var horaInicio = $("#txt-ag-inicio").val();
        var horaTermino = $("#txt-ag-termino").val();
        var data = $("#txt-data").val();

        var a = {
            "codAgendamento": codAgendamento,
            "codProcedimento": codProcedimento,
            "horaInicio": data + " " + horaInicio,
            "horaTermino": data + " " + horaTermino
        }

        return $.post("api/agendamentos/salvar", JSON.stringify(a), function () {
            clearAgendamentoForm();
            onSchedulerRefreshNeeded();
        });
    }

    function clearAgendamentoForm() {
        $("#cod-agendamento").val(0);
        $("#sel-servico").val(null);
        $("#txt-ag-inicio").val(null);
        $("#txt-ag-termino").val(null);
    }

    function onSchedulerRefreshNeeded(){
        calendar.refetchEvents();
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

            var select = $("#sel-profissional");
            select.find("option").remove();

            $.each(data, (i, item) => {
                select.append($("<option>", { value: item.codPessoa, text: item.nomePessoa }));
            })

            select.trigger("change");
        })

        $.when(q1, loadServicos()).then(() => {
            callBack();
        })
    }
});


