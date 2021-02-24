function pagaAgendamentos(data) {
    return $.post("/caixa/api/procedimentos-pagamento", JSON.stringify(data));
}

/** Classe que representa o cabeçalho de pagamento de agendamentos */
function AgendamentoPagamentoDTO(codCliente, codFormaPagamento, totalRecebido) {
    this.codCliente = codCliente;
    this.codFormaPagamento = codFormaPagamento;
    this.totalRecebido = totalRecebido;
    this.itens = [];
}

/** Classe que representa um procedimento a ser pago */
function AgendamentoPagamentoItemDTO(codAgendamentoItem, valorPago) {
    this.codAgendamentoItem = codAgendamentoItem;
    this.valorPago = valorPago;
}