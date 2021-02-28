$(document).ready(function () {
    $.ajaxSetup({ contentType: "application/json; charset=utf-8", cache: false });

    var pHora = $("#p-hora");

    var procedimentosDtTable = $('#dt-tb-procedimentos').DataTable({
        "columns": [
            {
                render: function (data, type, row) {
                    return "<div><input type='checkbox' class='ck-selitem'></input></div>";
                }
            },
            { "data": "dataAgendamento" },
            { "data": "codServico" },
            { "data": "servico" },
            {
                "data": "valorServico",
                "render": function (data, type, row) {
                    return Number(data).toFixed(2).toString();
                }
            }
        ],

        "scrollY": "300px",
        "scrollCollapse": true,
        "paging": false,
        "bInfo": false,
        "bFilter": false,
        "language": {
            "emptyTable": "Sem dados para mostrar"
        },
    });

    // Datatables aparece pequeno por causa do modal.
    $("#md-caixa").on("shown.bs.modal", function () {
        procedimentosDtTable.columns.adjust();
    })

    $("#md-caixa").on("hidden.bs.modal", function () {
        limparTelaCaixa();
    });

    $("input[type=number]").val(Number(0).toFixed(2).toString());

    // Inicia relógio
    setInterval(() => {
        pHora.html(moment().format('HH:mm:ss'))
    }, 1000);

    $("#btn-salvar-pagamento").click((e) => {

        e.preventDefault();

        let pagamento = new AgendamentoPagamentoDTO();
        let pagamentoItens = [];

        pagamento.codCliente = parseInt($("#sel-cliente-caixa").val(), 10);
        pagamento.totalRecebido = parseFloat($("#txt-recebido").val());
        pagamento.codFormaPagamento = 1;

        $("input:checked").each(function (index, el) {
            let row = $(el).closest('tr');

            pagamentoItens.push(new AgendamentoPagamentoItemDTO(procedimentosDtTable.row(row).data().codAgendamentoItem,
                procedimentosDtTable.row(row).data().valorServico));
        });

        pagamento.itens = pagamentoItens;

        pagaAgendamentos(pagamento)
            .then(() => {
                onPagamentoRealizado();
            })
            .fail((e) => {
                if (e.responseText) {
                    toastr["error"](e.responseText)
                }
                else {
                    toastr["error"](e.responseText);
                }
            });
    });

    loadClientes()
        .then(() => {
            $("#sel-cliente-caixa").on('change', () => {
                mostraSaldoCliente();
                mostraTotais();
                loadDataTables();
            });

            loadDataTables();
        })

    function loadDataTables() {
        procedimentosDtTable.ajax.url("/caixa/api/agendamentos/agendamentos-a-pagar/" +
            parseInt($("#sel-cliente-caixa").val())).load(() => {
                onTableLoad();
            });
    }

    function onPagamentoRealizado() {
        Swal.fire(
            '',
            'Pagamento realizado!',
            'success'
        ).then(() => {
            loadClientes()
                .then(() => {
                    $("#md-caixa").modal('hide');
                })
        })
    }

    function onTableLoad() {
        $(".ck-selitem").on('change', function (e) {
            mostraTotais();
        });

        $(".ck-selitem").trigger('change');
    }

    function mostraTotais() {
        let total = 0;

        $("input:checked").each(function (index, el) {
            var row = $(el).closest('tr');

            total += procedimentosDtTable.row(row).data().valorServico;
        });

        $("#txt-total").val(Number(total).toFixed(2).toString());
    }

    function mostraSaldoCliente() {
        obterSaldoCliente($("#sel-cliente-caixa").val())
            .then(function (response) {
                $("#txt-saldo").html(Number(response).toFixed(2).toString());

                if (response < 0) {
                    $(".col-saldo .card")
                        .removeClass("border-success")
                        .addClass("border-danger");
                }
                else {
                    $(".col-saldo .card")
                        .removeClass("border-danger")
                        .addClass("border-success");
                }
            })
    }       

    function limparTelaCaixa() {
        $("#sel-cliente-caixa,#txt-recebido,#txt-novo-saldo,#txt-total").val(0);
        $("#sel-cliente-caixa").trigger("change");
    }
});

// Carrega lista de clients com valores em aberto
function loadClientes() {

    return $.get("/caixa/api/pessoas", (data) => {

        var select = $("#sel-cliente-caixa");
        select.find("option").remove();

        select.append($("<option>", { value: 0, text: null }));

        $.each(data, (i, item) => {
            select.append($("<option>", { value: item.codPessoa, text: item.nomePessoa }));
        });
    })
};

// Inicializa e abre a janela do caixa
function caixa() {
    loadClientes()
        .then(() => {
            var cx = $("#md-caixa");

            cx.draggable();
            cx.modal('show');
        })
}
