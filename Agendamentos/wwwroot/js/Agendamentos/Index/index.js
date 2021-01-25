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
                //pop.draggable();
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

    $("#sel-profissional").on('change', () => {
        loadProcedimentos();
    });

    $("#sel-procedimento").on('change', (e, b) => {
        var selected = $("#sel-procedimento").find('option:selected');
        var inicio = selected.attr('data-horaInicio');
        var termino = selected.attr('data-horaTermino');

        $("#txt-ag-inicio").val(inicio);
        $("#txt-ag-termino").val(termino);
    });

    $("#sel-status").prop("disabled", true);

    function onAgendamentoSalvar() {

        var codAgendamento = parseInt($("#cod-agendamento").val());
        var codProcedimento = parseInt($("#sel-procedimento").val());
        var horaInicio = $("#txt-ag-inicio").val();
        var horaTermino = $("#txt-ag-termino").val();

        var a = {
            "codAgendamento": codAgendamento,
            "codProcedimento": codProcedimento,
            "horaInicio": horaInicio,
            "horaTermino": horaTermino
        }

        return $.post("api/agendamentos/salvar", JSON.stringify(a), function () {
            onSchedulerRefreshNeeded();
        });
    }

    function onSchedulerRefreshNeeded(){
        calendar.refetchEvents();
    }

    // Carrega os procedimetos do profiossinal
    function loadProcedimentos() {
        var select = $("#sel-procedimento");
        var selProcCode = $("#sel-profissional").val();

        select.find("option").remove();

        if (!selProcCode) {
            return $.Deferred().resolve();
        }

        return $.get("api/procedimentos/" + selProcCode, (data) => {
            $.each(data, (i, item) => {
                var opt = $("<option>", { value: item.codProcedimento, text: item.itemDisplay });

                opt.attr("data-horaInicio", item.num_HoraInicio);
                opt.attr("data-horaTermino", item.num_HoraFim);

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

        $.when(q1, loadProcedimentos()).then(() => {
            callBack();
        })
    }
});


