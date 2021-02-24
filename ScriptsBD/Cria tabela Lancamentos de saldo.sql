CREATE TABLE SaldoPessoa
(
   Cd_LancSaldo int not null identity,
   Cd_Pessoa int not null,
   Dat_Lancamento Datetime not null,
   Num_ValorLancamento numeric(8,2),
   primary key(Cd_LancSaldo),
   foreign key(Cd_Pessoa) references Pessoas(cod_pessoa)
)