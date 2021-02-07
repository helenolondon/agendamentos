CREATE TABLE [dbo].[ProfissionaisProcedimentos](
	[Cd_Procedimento] [int] IDENTITY(1,1) NOT NULL,
	[Cd_Pessoa] [int] NOT NULL,
	[Cd_Servico] [int] NOT NULL,
	[Cd_DiaSemana] [int] NOT NULL,
	[Num_HoraInicio] [time](7) NOT NULL,
	[Num_HoraFim] [time](7) NOT NULL,
	[Num_Comissao] [numeric](18, 2) NOT NULL,
 CONSTRAINT [PK_ProfissionaisProcedimentos] PRIMARY KEY CLUSTERED 
(
	[Cd_Procedimento] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ProfissionaisProcedimentos]  WITH CHECK ADD FOREIGN KEY([Cd_Pessoa])
REFERENCES [dbo].[pessoas] ([cod_pessoa])
GO

ALTER TABLE [dbo].[ProfissionaisProcedimentos]  WITH CHECK ADD FOREIGN KEY([Cd_Pessoa])
REFERENCES [dbo].[pessoas] ([cod_pessoa])
GO

ALTER TABLE [dbo].[ProfissionaisProcedimentos]  WITH CHECK ADD  CONSTRAINT [FK_ProfisProcPessoas] FOREIGN KEY([Cd_Pessoa])
REFERENCES [dbo].[pessoas] ([cod_pessoa])
GO

ALTER TABLE [dbo].[ProfissionaisProcedimentos] CHECK CONSTRAINT [FK_ProfisProcPessoas]
GO

ALTER TABLE [dbo].[ProfissionaisProcedimentos]  WITH CHECK ADD  CONSTRAINT [FK_ProfisProcServ] FOREIGN KEY([Cd_Servico])
REFERENCES [dbo].[Servicos] ([ID_servico])
GO

ALTER TABLE [dbo].[ProfissionaisProcedimentos] CHECK CONSTRAINT [FK_ProfisProcServ]
GO
