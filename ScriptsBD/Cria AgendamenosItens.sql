USE [Odontica]
GO

/****** Object:  Table [dbo].[AgendamentosItens]    Script Date: 01/02/2021 19:31:41 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[AgendamentosItens](
	[Cd_AgendamentoItem] [int] NOT NULL,
	[Cd_Agendamento] [int] NOT NULL,
	[Cd_Servico] [int] NOT NULL,
	[Cd_Profissional] [int] NOT NULL,
	[Dat_Inicio] [datetime] NOT NULL,
	[Dat_Termino] [datetime] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Cd_AgendamentoItem] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[AgendamentosItens]  WITH CHECK ADD FOREIGN KEY([Cd_Agendamento])
REFERENCES [dbo].[Agendamentos] ([Cd_Agendamento])
GO

ALTER TABLE [dbo].[AgendamentosItens]  WITH CHECK ADD FOREIGN KEY([Cd_Agendamento])
REFERENCES [dbo].[Agendamentos] ([Cd_Agendamento])
GO

ALTER TABLE [dbo].[AgendamentosItens]  WITH CHECK ADD FOREIGN KEY([Cd_Profissional])
REFERENCES [dbo].[pessoas] ([cod_pessoa])
GO

ALTER TABLE [dbo].[AgendamentosItens]  WITH CHECK ADD FOREIGN KEY([Cd_Profissional])
REFERENCES [dbo].[pessoas] ([cod_pessoa])
GO

ALTER TABLE [dbo].[AgendamentosItens]  WITH CHECK ADD FOREIGN KEY([Cd_Servico])
REFERENCES [dbo].[Servicos] ([ID_servico])
GO

ALTER TABLE [dbo].[AgendamentosItens]  WITH CHECK ADD FOREIGN KEY([Cd_Servico])
REFERENCES [dbo].[Servicos] ([ID_servico])
GO

CREATE INDEX IDX_AGITENSDATA ON AGENDAMENTOSITENS(DAT_INICIO, DAT_TERMINO)
GO

CREATE INDEX IDX_AGITENSPROF ON AGENDAMENTOSITENS(CD_PROFISSIONAL)
GO

