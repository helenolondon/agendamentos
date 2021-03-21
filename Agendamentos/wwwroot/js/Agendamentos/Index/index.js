document.addEventListener('DOMContentLoaded', function () {
    // Configura requisições http
    $.ajaxSetup({ contentType: "application/json; charset=utf-8" });

    var form;
    var calendarEl = document.getElementById('calendar');
    var calendar = null;
    var configuracoes = {};

    // Inicializa a agenda
    init();

    /**
    * Eventos da aplicação
    * */

    // Evento que ocorre quando a tela do caixa é finalizado
    function onCaixaClosed() {
        onSchedulerRefreshNeeded();
    }

    // Evento que ocorre quando o usuário clica em adicionar agendamento
    function onNovoAgendamento() {
        clearAgendamentoForm();
        addProcedimentoItem();

        $("#txt-data")[0].valueAsDate = new Date();
        $("input, select, textarea").attr("disabled", false);
        $("#btn-salvar").attr("disabled", false);
    }

    // Ocorre quando o usuário remove um agendamento
    function onRemoverAgendamento(codAgendamentoItem) {

        var md = $("#md-confirma");
        md.find(".modal-body").html("<span>Confirma a exclusão do procedimento?</span>");
        md.draggable();
        md.find("#btn-sim").off('click');
        md.find("#btn-sim").on('click', () => {
            var q = $.ajax({
                url: 'agendamentos/api/agendamentos/remover?codAgendamentoItem=' + codAgendamentoItem,
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

    // Ocorre quando o usuário salva um agendamento
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

            "Itens": retAgendamentoItens(codAgendamento, codCliente)
        }

        mostrarSalvando();

        return $.post("agendamentos/api/agendamentos/salvar", JSON.stringify(request), function () {
            clearAgendamentoForm();
            onSchedulerRefreshNeeded();
        }).fail((resp) => {
            if (resp.responseJSON) {
                setValidacaoRemota(null);

                $.each(resp.responseJSON, (index, item) => {
                    setValidacaoRemota(item);
                })
            }
        }).always(() => {
            mostrarSalvo();
        })

        function mostrarSalvando() {
            $("#btn-salvar")
                .attr('disabled', true)
                .html('Salvando');

            $("#btn-cancelar").attr('disabled', true);
        };

        function mostrarSalvo() {
            $("#btn-salvar")
                .attr('disabled', false)
                .html('Salvo');

            $("#btn-cancelar").attr('disabled', false);
        }
    }

    // Ocorre quando os dados da agenda precisam ser atualizados na tela
    function onSchedulerRefreshNeeded() {
        calendar.refetchEvents();
    }

    // Ocorre quando o usuário edita um agendamento
    function onEditarAgendamento(e) {

        var pop = $('#md-editar-agendamento');
        var agendamento = consultarAgendamento(e.codAgendamento)
            .then((data) => {
                agendamento = data
            });

        setPopAgendamentoTitle("Editar Agendamento");
        clearAgendamentoForm();

        $.when(agendamento, loadClientes(), loadServicos())
            .then(() => {

                let realizado = statusEREalizado(e.codStatus)

                // Cabeçalho
                $("#cod-agendamento").val(e.codAgendamento);
                $("#sel-cliente").val(e.codCliente);
                $("#txt-data")[0].valueAsDate = new Date(e.dataAgendamento);
                $("#sel-status").val(e.codStatus);

                $.each(agendamento.itens, (index, item) => {

                    addProcedimentoItem()
                        .then((novoItem) => {
                            $(".procedimento-item[data-id=" + novoItem + "]").attr("data-cod-procedimento", item.codAgendamentoItem);

                            $("#sel-servico-" + novoItem).val(item.codServico);

                            $("#txt-ag-inicio-" + novoItem).val(item.horaInicio);
                            $("#txt-ag-termino-" + novoItem).val(item.horaTermino);
                            $("#txa-observacoes-" + novoItem).val(item.observacao);

                            loadProfissionais(novoItem)
                                .then(() => {
                                    $("#sel-profissional-" + novoItem).val(item.codProfissional);
                                });

                            pop.draggable();
                            pop.modal();
                        })
                });

                // Se o status for realizado, só é permitido consultar
                $("input, select, textarea").attr("disabled", realizado);
                $("#btn-salvar").attr("disabled", realizado);

                // Não é permitido alterar o cliente
                $("#sel-cliente").attr("disabled", true);

                $(".collapse.show").each((index, el) => {
                    if (index == 0) {
                        $(el).collapse('show')
                    } else {
                        $(el).collapse('hide');
                    }
                });
            });
    }

    /**
     * Funções utilizadas pelo calendário
     * */

    // Executa inicializações
    async function init() {

        // Carrega filtros de Profissionais
        await carregaFiltrosProfissionais();
        await carregaConfiguracoes();

        // Inicializa o calendário
        calendar = inicializaCalendario();
        calendar.render();

        // Mostra a área emm volta do horári atual;
        calendar.scrollToTime(moment(new Date().getTime()).format("H:mm"));

        form = $("form").submit(function (event) {
            event.preventDefault();

            if (!$("form")[0].checkValidity()) {

                form[0].classList.add('was-validated');
                event.stopPropagation();

                return;
            }

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
            collapseProcedimentos();
            addProcedimentoItem();
        })

        $("#txt-data").on('change', function () {
            $(".procedimento-item")
                .find("input[type='time']")
                .first()
                .trigger('change');
        })

        // Evento ocorre quando o filtro de funcionários muda.
        $("#func-tabs").on("shown.bs.tab", function (event) {
            onSchedulerRefreshNeeded();
        })
    }

    // Configura e retorna objeto calendário inicializado
    function inicializaCalendario() {
        let diasDisp = retDiasAgenda();

        if (diasDisp.length == 7) {
            Swal.fire(
                '',
                'A agenda está desabilitada',
                'info'
            );

            return;
        }

        return new FullCalendar.Calendar(calendarEl, {
            initialView: 'timeGridWeek',
            timeZone: 'America/Sao_Paulo',
            locale: 'pt-br',
            weekends: true,
            slotDuration: '00:15:00',
            slotLabelInterval: '00:30',
            scrollTime: '08:00:00',
            editable: false,
            hiddenDays: diasDisp,
            slotMinTime: configuracoes.funcInicio,
            slotMaxTime: configuracoes.funcFinal,
            slotLaneClassNames: function (args) {
                let dataSlot = "1900-01-01T" + moment(args.date).utc().format("HH:mm");
                let inicioAlmoco = "1900-01-01T" + configuracoes.almocInicio;
                let finalAlmoco = "1900-01-01T" + configuracoes.almocFinal;

                if (moment(dataSlot).isSameOrAfter(inicioAlmoco) &&
                    moment(dataSlot).isBefore(finalAlmoco)) {

                    return "hora-almoco";
                }
            },
            events: {
                url: '/agendamentos/api/agendamentos',
                extraParams: function () {
                    return {
                        'codProfissional': $(".nav-link.active").attr("data-func-id")
                    }
                }
            },
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
                },
                caixa: {
                    text: 'Caixa',
                    click: abreCaixa
                }
            },
            headerToolbar: {
                left: 'prev,next,today',
                center: 'title',
                right: 'novoAgendamento,caixa,dayGridDay,timeGridWeek,dayGridMonth'
            },
            eventContent: function (arg) {

                if (arg.view.type == 'timeGridWeek') {

                    let span = document.createElement("span");
                    span.innerHTML = "X";
                    span.style = "float: right; margin-right: 6px; font-size: 10px"
                    span.onclick = ((ev, e) => {
                        onRemoverAgendamento(arg.event.extendedProps.codAgendamentoItem);
                    });

                    let ElStatus = document.createElement('p')
                    ElStatus.style = "font-size: 11px; margin: 10px 2px 2px;"

                    if (arg.event.extendedProps.pago == "S") {
                        ElStatus.innerHTML = "Status: " + arg.event.extendedProps.status + " (Pago)";
                    }
                    else {
                        ElStatus.innerHTML = "Status: " + arg.event.extendedProps.status;
                    }

                    let ElHorario = document.createElement('p')

                    ElHorario.innerHTML = "Horário: " + arg.event.extendedProps.horarioLabel;
                    ElHorario.style = "margin: 0px 2px 2px; font-size: 11px;"

                    let ElCliente = document.createElement('p')

                    ElCliente.innerHTML = "Cliente: " + arg.event.extendedProps.nomeCliente;
                    ElCliente.style = "margin: 0px 2px 2px; font-size: 11px;"

                    let ElProfissional = document.createElement('p')
                    ElProfissional.style = "font-size: 11px; margin: 0px 2px 2px;"

                    ElProfissional.innerHTML = "Profissional: " + arg.event.extendedProps.nomeProfissional;

                    let ElServico = document.createElement('p')
                    ElServico.style = "font-size: 11px; margin: 0px 2px 2px;"

                    ElServico.innerHTML = "Procedimento: " + arg.event.extendedProps.servico;

                    let arrayOfDomNodes = [span, ElStatus, ElHorario, ElCliente, ElProfissional, ElServico]
                    return { domNodes: arrayOfDomNodes }
                }
                else if (arg.view.type == 'dayGridMonth') {

                    let ElAgendado = document.createElement('p')

                    ElAgendado.innerHTML = arg.event.extendedProps.horarioLabel.substring(0, 5) + "-" + arg.event.extendedProps.nomeCliente;;
                    ElAgendado.style = "margin: 0px 2px 2px; font-size: 09px; overflow: hidden;"

                    let arrayOfDomNodes = [ElAgendado]
                    return { domNodes: arrayOfDomNodes }
                }
            },
            eventClick: function (e) {
                if (e.jsEvent.layerY > 10) {
                    onEditarAgendamento(e.event.extendedProps);
                }
            }
        });
    }

    // Adicion a um novo agendamento
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

    // Abre a tela do caixa
    function abreCaixa() {
        let mdCx = $("#md-caixa");

        mdCx.on("hidden.bs.modal", function () {
            onCaixaClosed();
        });

        caixa();
    }

    // Altera o título do modal de editar agendamento
    function setPopAgendamentoTitle(titulo) {
        $("#md-editar-agendamento")
            .find(".modal-title")
            .html(titulo);
    }

    function collapseProcedimentos() {
        $(".collapse").collapse('hide');
    }

    // Adiciona item de agendamento
    function addProcedimentoItem() {
        const procedimentoTemplate = ({ id, codProcedimentoItem }) => retProcedimentoTemplate(id, codProcedimentoItem);

        let novaId = 1;

        let cl = $(".procedimento-item").first();

        if (cl.length > 0) {
            novaId = parseInt(cl.attr("data-id")) + 1;
        };

        $('#novo-procedimento').prepend([
            { id: novaId, codProcedimentoItem: '0' }

        ].map(procedimentoTemplate).join(''));

        $(".btn-remover-item").click(function () {
            if ($(".procedimento-item").length > 1) {
                $(this).parent().parent().remove();
            }

            botoesRemoverDisableEnable();
        });

        $("#collapse-content-" + novaId).on("show.bs.collapse", () => {
            $("#btn-collape-item-" + novaId).html("<i class='bi bi-arrows-angle-contract'>");
        });

        $("#collapse-content-" + novaId).on("hide.bs.collapse", () => {
            $("#btn-collape-item-" + novaId).html("<i class='bi bi-arrows-angle-expand'>");
        });

        return loadServicos(novaId)
            .then(() => {
                $([]).add("#sel-servico-" + novaId)
                    .add("#txt-ag-inicio-" + novaId)
                    .add("#txt-ag-termino-" + novaId)
                    .on('change', (e, b) => {
                        loadProfissionais(novaId);
                    });
                botoesRemoverDisableEnable();

                return $.Deferred().resolve(novaId);
            });
    }

    function botoesRemoverDisableEnable() {
        if ($(".procedimento-item").length == 1) {
            $(".btn-remover-item").prop('disabled', true);
        }
        else {
            $(".btn-remover-item").prop('disabled', false);
        }
    }

    // Limpa formulário de agendamento
    function clearAgendamentoForm() {
        $("#cod-agendamento").val(0);
        $("#txt-data").val(null);
        $("#sel-cliente").val(null);
        $("#sel-status").val(1);

        $(".procedimento-item").remove();
        setValidacaoRemota(null);

        $("form").removeClass("was-validated");
    }

    // Constroi vetor de itens de agendamentos para ser enviado para o servidor
    function retAgendamentoItens(codAgendamento, codCliente) {
        let amat = [];

        $(".procedimento-item").each(function (i) {
            let id = $(this).attr("data-id");

            let codServico = parseInt($("#sel-servico-" + id).val());
            let codProfissional = parseInt($("#sel-profissional-" + id).val());
            let horaInicio = $("#txt-ag-inicio-" + id).val();
            let horaTermino = $("#txt-ag-termino-" + id).val();
            let data = $("#txt-data").val();
            let codAgendamentoItem = parseInt($(this).attr("data-cod-procedimento"));
            let observacoes = $("#txa-observacoes-" + id).val();

            if (Number.isNaN(codAgendamentoItem)) {
                codAgendamentoItem = 0;
            }

            amat.push({
                "CodAgendamentoItem": codAgendamentoItem,
                "CodAgendamento": codAgendamento,
                "Inicio": data + "T" + horaInicio,
                "Termino": data + "T" + horaTermino,
                "codCliente": + codCliente,
                "CodServico": codServico,
                "CodProfissional": codProfissional,
                "Observacao": observacoes
            });
        })

        return amat;
    }

    // Verifica se o status do agendamento é "Realizado"
    function statusEREalizado(status) {
        return status == 3;
    }

    function consultarAgendamento(codAgendamento) {
        return $.get("agendamentos/api/agendamentos/" + codAgendamento);
    }

    // Carrega os profiossionais disponíveispara o agendamento
    function loadProfissionais(id) {

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

        return $.get("agendamentos/api/procedimentos/listar-profissionais-agendamento", request, (data) => {
            var select = $("#sel-profissional-" + id);
            select.find("option").remove();

            $.each(data, (i, item) => {
                select.append($("<option>", { value: item.codPessoa, text: item.nomePessoa }));
            });
        });
    }

    // Carrega profissionais nas tabs de filtro
    async function carregaFiltrosProfissionais() {
        let codAtivo = $(".nav-link.active").attr("data-func-id");

        $("#func-tabs").find(".nav-p-filtro").remove();

        await $.get("agendamentos/api/profissionais")
            .then(function (data) {
                data.forEach(function (item, index, array) {
                    $("#func-tabs").append(`
                        <li class="nav-item nav-p-filtro" role="presentation">
                            <a class="nav-link" id="func-${item.codPessoa}" data-func-id="${item.codPessoa}" data-toggle="tab" href="#${item.nomePessoa}" role="tab" aria-controls="contact" aria-selected="false">${item.nomePessoa}</a>
                        </li>
                    `);
                });

                $(".nav-link[data-func-id=" + codAtivo + "]").addClass("active");
            });
    }

    // Carrega os procedimetos do profiossinal
    function loadServicos(id) {
        var select = $("#sel-servico-" + id);

        select.find("option").remove();

        return $.get("agendamentos/api/servicos", (data) => {
            $.each(data, (i, item) => {
                var opt = $("<option>", { value: item.codServico, text: item.nomeServico });

                select.append(opt);
            });

            select.trigger('change');
        });
    }

    // Carrega clientes
    function loadClientes() {
        return $.get("agendamentos/api/pessoas", (data) => {

            var select = $("#sel-cliente");
            select.find("option").remove();

            $.each(data, (i, item) => {
                select.append($("<option>", { value: item.codPessoa, text: item.nomePessoa }));
            })
        })
    }

    // Mostra mensagem de erro na parte inferior do modal de editar agendamento
    function setValidacaoRemota(erro) {

        if (!erro) {
            $("#salvar-erros").html(null);
            return;
        }

        $("#salvar-erros").append("<p>" + erro + "</p>")
    }

    // Retorna string de template de novo agendamento
    function retProcedimentoTemplate(id, codProcedimentoItem) {
        return `
<div class="procedimento-item border-top-10" data-id=${id} data-cod-procedimento=${codProcedimentoItem}>
    <div id="collapse-content-${id}" class="collapse show">
        <!--Hora de início e fim-->
        <div class="container">
            <div class="row .no-gutters">
                <div class="col">
                    <div class="form-group">
                        <label for=txt-ag-inicio-${id}>Início:</label>
                        <input required type="time" class="form-control" id=txt-ag-inicio-${id}>
                    </div>
                </div>
                <div class="col">
                    <div class="form-group">
                        <label for=txt-ag-termino-${id}>Término:</label>
                        <input required type="time" class="form-control" id=txt-ag-termino-${id}>
                    </div>
                </div>
            </div>
        </div>

        <!--Procedimento-->
        <div class="form-group">
            <label for=sel-servico-${id}>Procedimento:</label>
            <select required id=sel-servico-${id} class="custom-select">
                <option selected></option>
            </select>
        </div>

        <!--Profissional-->
        <div class="form-group">
            <label for=sel-profissional-${id}>Profissional:</label>
            <select required id=sel-profissional-${id} class="custom-select profissional">
                <option selected></option>
            </select>
        </div>
        
        <!-- Observações do agendmento -->
        <div class="form-group">
            <label for="txa-observacoes-${id}">Observações</label>
            <textarea class="form-control" id="txa-observacoes-${id}" rows="3"></textarea>
        </div>

    </div>
</div>
    `;
    }

    // Carrega as configurações da agenda definidas pelo usuário
    async function carregaConfiguracoes() {
        await $.get("agendamentos/api/agendamentos/configuracoes", null, function (data) {
            configuracoes = data;
        }).fail(function () {
            Swal.fire(
                '',
                'Não foi possível carregar as configurações da agenda',
                'error'
            );
        });
    }

    // Retorna os dias em qua a agenda está disponível
    function retDiasAgenda() {
        let dias = [];

        if (configuracoes.dispDomingo == 0) {
            dias.push(0);
        }
        if (configuracoes.dispSegunda == 0) {
            dias.push(1);
        }
        if (configuracoes.dispTerca == 0) {
            dias.push(2);
        }
        if (configuracoes.dispQuarta == 0) {
            dias.push(3);
        }
        if (configuracoes.dispQuinta == 0) {
            dias.push(4);
        }
        if (configuracoes.dispSexta == 0) {
            dias.push(5);
        }
        if (configuracoes.dispSabado == 0) {
            dias.push(6);
        }

        return dias;
    }
});