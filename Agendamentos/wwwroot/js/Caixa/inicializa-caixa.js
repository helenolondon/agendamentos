﻿document.addEventListener('DOMContentLoaded', function () {
    $(document).ready(function () {

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
        
        $("input[type=number]").val(Number(0).toFixed(2).toString());

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
                $("#md-caixa").modal('hide');
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
    });
});
