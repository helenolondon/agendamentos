CREATE TABLE ProfissionaisCompromissos
(
  CodCompromisso INT NOT NULL IDENTITY,
  Descricao VARCHAR(500) NOT NULL,
  Dat_Inicio DATETIME NOT NULL,
  Dat_Termino DATETIME NOT NULL,
  Cd_Tipo INT NOT NULL,
  Cd_Pessoa INT NOT NULL,

  PRIMARY KEY(CodCompromisso),
  FOREIGN KEY (Cd_Pessoa) REFERENCES Pessoas (Cod_Pessoa)
)