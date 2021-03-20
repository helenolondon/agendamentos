CREATE TABLE AgendamentosConfiguracoes
(
  CodConfiguracao int not null,
  Tim_FuncInicio Time not null,
  Tim_FuncFinal Time not null,
  Tim_AlmocInicio Time not null,
  Tim_AlmocFinal Time not null,
  Num_BloqAlmoco int not null CHECK (Num_BloqAlmoco IN (0,1)),
  Num_DispSegunda int not null CHECK (Num_DispSegunda IN (0,1)),
  Num_DispTerca int not null CHECK (Num_DispTerca IN (0,1)),
  Num_DispQuarta int not null CHECK (Num_DispQuarta IN (0,1)),
  Num_DispQuinta int not null CHECK (Num_DispQuinta IN (0,1)),
  Num_DispSexta int not null CHECK (Num_DispSexta IN (0,1)),
  Num_DispSabado int not null CHECK (Num_DispSabado IN (0,1)),
  Num_DispDomingo int not null CHECK (Num_DispDomingo IN (0,1)),

  PRIMARY KEY(CodConfiguracao)
)

GO

INSERT INTO AgendamentosConfiguracoes
VALUES
(
  1,
  '07:00',
  '19:00',
  '12:00',
  '13:00',
  1,
  1,
  1,
  1,
  1,
  1,
  1,
  0
)

