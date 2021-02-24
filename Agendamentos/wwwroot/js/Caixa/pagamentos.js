function pagaAgendamentos(data) {
    return $.post("/caixa/api/procedimentos-pagamento", JSON.stringify(data));
}

function obterSaldoCliente(codCliente) {
    return $.get("/caixa/api/pessoas/obter-saldo/" + codCliente);
}

function adicionarSaldoCliente(request) {
    return $.post("/caixa/api/pessoas/adicionar-saldo", JSON.stringify(request));
}

/** Classe de request para adicionar saldo a clientes */
function AdicionaSaldoClienteRequest(codCliente, valorSaldo) {
    this.codPessoa = codCliente;
    this.valor = valorSaldo;
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