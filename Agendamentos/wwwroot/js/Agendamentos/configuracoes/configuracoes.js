$(document).ready(function () {
    $.ajaxSetup({ contentType: "application/json; charset=utf-8" });
    let compromissos = new compromissosCtrl();
    let url = "/agendamentos/api/agendamentos/configuracoes";
    let urlCompromissos = "/agendamentos/agendamentos/compromissos";

    // Carrega configuracoes
    init();

    /**
     *  Eventos da página
     * 
     */

    // Quando o servidor retorna as configurações e estão prontas para serem exibidas
    function onLoadConfiguracoes(conf) {
        clearForm();

        if (!conf) {
            return;
        }

        funcInicio(conf.funcInicio);
        funcFim(conf.funcFinal);

        funcAlmocoInicio(conf.almocInicio);
        funcAlmocoFim(conf.almocFinal);

        bloqAlmoco(conf.bloqAlmoco);

        dispDiaSemana("Segunda", conf.dispSegunda);
        dispDiaSemana("Terca", conf.dispTerca);
        dispDiaSemana("Quarta", conf.dispQuarta);
        dispDiaSemana("Quinta", conf.dispQuinta);
        dispDiaSemana("Sexta", conf.dispSexta);
        dispDiaSemana("Sabado", conf.dispSabado);
        dispDiaSemana("Domingo", conf.dispDomingo);
    }

    function onPostConfiguracoes() {
        let request = getConfiguracoesRequest();

        mostraSalvando();

        $.post(url, JSON.stringify(request), function () {
            Swal.fire(
                '',
                'Configurações salvas!',
                'success'
            ).then(() => {
                loadConfiguracoes();
            })
        }).fail(function () {
            Swal.fire(
                '',
                'Não foi possível salvar as configurações',
                'error'
            );
        }).always(function () {
            mostraSalvo();
        });
    }

    function onCompromissosTabShow() {        
        compromissos.init();
    }

    /**
     * Funções usadas na página
     * */

    // Inicializa a página
    function init() {
        loadConfiguracoes();
        loadCompromissos();
        addHandlers();        
    }

    // Conecta eventos dos objetos da página
    function addHandlers() {
        $("#btn-salvar").click(function (e) {
            e.preventDefault();

            onPostConfiguracoes();
        });

        $("#tabs-configuracoes").on('shown.bs.tab', function () {
            if ($("#tabs-configuracoes-content .tab-pane.active").attr("id") == "compromissos") {
                onCompromissosTabShow();
            }
        });
    }

    // Obtém as configuraçõs atuais e mostra na tela.
    function loadConfiguracoes() {
        $.get(url, null, function (data) {
            onLoadConfiguracoes(data);
        }).fail(function () {
            Swal.fire(
                '',
                'Não foi possível carregar as configurações',
                'error'
            );
        })
    }

    // Carrega o conteúdo da tab de compromissos
    function loadCompromissos() {
        $("#compromissos")
            .load(urlCompromissos)
            .ajaxError(function () {
                Swal.fire(
                    '',
                    'Ocorreu uma falha ao carregar os compromissos',
                    'error'
                );
            });
    }

    // Limpa os dados do formulário
    function clearForm() {
        $("input").val(null);
    }

    // Monta o request do post das configurações
    function getConfiguracoesRequest() {
        return {
            "funcInicio": funcInicio(),
            "funcFinal": funcFim(),
            "almocInicio": funcAlmocoInicio(),
            "almocFinal": funcAlmocoFim(),
            "bloqAlmoco": bloqAlmoco(),
            "dispSegunda": dispDiaSemana("Segunda"),
            "dispTerca": dispDiaSemana("Terca"),
            "dispQuarta": dispDiaSemana("Quarta"),
            "dispQuinta": dispDiaSemana("Quinta"),
            "dispSexta": dispDiaSemana("Sexta"),
            "dispSabado": dispDiaSemana("Sabado"),
            "dispDomingo": dispDiaSemana("Domingo")
        };
    }

    // Disabilita botão de salvar para evitar duplo click
    function mostraSalvando() {
        $("#btn-salvar")
            .attr('disabled', true)
            .html("Salvando aguarde...");
    }

    // Habilita botão de salvar
    function mostraSalvo() {
        $("#btn-salvar")
            .attr('disabled', false)
            .text("Salvo");
    }

    /**
     * Gets e Sets usados na página
     * 
     */

    // Get e Set dataa de Início de funcionamento
    function funcInicio(valor) {
        let c = $("#txt-inicio");

        if (c.length == 0) {
            return null;
        }

        if (valor == null || valor == undefined) {
            return c.val();
        }

        c.val(valor);
    }

    // Get e Set final data de funcionamento
    function funcFim(valor) {
        let c = $("#txt-final");

        if (c.length == 0) {
            return null;
        }

        if (valor == null || valor == undefined) {
            return c.val();
        }

        c.val(valor);
    }

    // Get e Set dataa de Início de almoço
    function funcAlmocoInicio(valor) {
        let c = $("#txt-almoco-inicio");

        if (c.length == 0) {
            return null;
        }

        if (valor == null || valor == undefined) {
            return c.val();
        }

        c.val(valor);
    }

    // Get e Set final data de almoço
    function funcAlmocoFim(valor) {
        let c = $("#txt-almoco-final");

        if (c.length == 0) {
            return null;
        }

        if (valor == null || valor == undefined) {
            return c.val();
        }

        c.val(valor);
    }

    // Get e Set bloqueio de almoço
    function bloqAlmoco(valor) {
        let c = $("#chkBloquearAlmoco");

        if (c.length == 0) {
            return null;
        }

        if (valor == null || valor == undefined) {
            return (Number)(c.prop("checked"));
        }

        c.prop("checked", valor);
    }

    // Get e Set dias da semana
    function dispDiaSemana(dia, valor) {
        let c = $("#chkDisp"+dia);

        if (c.length == 0) {
            return null;
        }

        if (valor == null || valor == undefined) {
            return (Number)(c.prop("checked"));
        }

        c.prop("checked", valor);
    }
})