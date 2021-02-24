--Create table AgendamentosFormaPagamento
--(
--  Cd_FormaPagamento int not null,
--  FormaPagamento varchar(80) not null,
--  primary key (Cd_FormaPagamento)
--)
--go

--INSERT INTO AgendamentosFormaPagamento VALUES (1, 'Dinheiro')



--CREATE TABLE AgendamentosPagamento
--(
--  Cd_Pagamento int not null identity,
--  Dat_Pagamento datetime not null,
--  Num_SaldoAnterior decimal(8,2),
--  Num_TotalRecebido decimal(8,2) not null,
--  Cd_FormaPagamento int not null,

--  primary key(Cd_Pagamento),
--  foreign key(Cd_FormaPagamento) references AgendamentosFormaPagamento(Cd_FormaPagamento)
--)
-- go

 CREATE TABLE AgendamentosPagamentoItens
(
  Cd_PagamentoItem int not null identity,
  Cd_Pagamento int not null,
  Cd_Servico int not null,
  Dat_Procedimento DateTime not null,
  Cd_AgendamentoItem int not null,
  Num_ValorServico decimal(8,2),
  Num_ValorPago decimal(8,2) not null,
  DescricaoServico varchar(255) not null,

  primary key(Cd_PagamentoItem),
  foreign key(Cd_AgendamentoItem) references AgendamentosItens(Cd_AgendamentoItem),
  foreign key(Cd_Pagamento) references AgendamentosPagamento(Cd_Pagamento),
  foreign key(Cd_Servico) references Servicos(Id_Servico)
)