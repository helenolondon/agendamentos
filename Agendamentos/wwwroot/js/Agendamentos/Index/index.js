document.addEventListener('DOMContentLoaded', function () {
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
                pop.draggable();
                pop.modal();
            });
        }
    }

    dialog = $("#dialog-form").dialog({
        autoOpen: false,
        height: 400,
        width: 350,
        modal: true,
        buttons: {
            "Salvar": function () { },
            "Cancelar": function () {
                dialog.dialog("close");
            }
        },
        close: function () {
            form[0].reset();
        }
    });

    form = $("form").submit(function (event) {
        event.preventDefault();
        alert("Form Salvo")
    });

    $("#btn-salvar").on("click", () => {
        form.submit()
    })

    $("#btn-cancelar").on('click', () => {
        form[0].reset();
    })

    $("#sel-profissional").on('change', () => {
        loadProcedimentos();
    });

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
                select.append($("<option>", { value: item.codProcedimento, text: item.itemDisplay }));
            });
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


