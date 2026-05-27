create database Loja;
go

use Loja;
go

create table Produtos(
ID_Produto int primary key identity(1,1),
Nome varchar(50) not null,
ValorCusto decimal(10,2) not null,
ValorVenda decimal(10,2) not null
);
go

-- INSERTS DESAFIO
INSERT INTO Produtos (Nome, ValorCusto, ValorVenda) VALUES
('Smartphone', 200.00, 400.00);

INSERT INTO Produtos (Nome, ValorCusto, ValorVenda) VALUES
('Café', 5.00, 10.00);

-- Atualização ValorVenda de Cafe para R$15 na tabela Loja.Produto
update dbo.Produtos
set ValorVenda = 15.00
where Nome = 'Café'

-- Cinco produtos mais caros da tabela "Loja.Produto" ordenados em ordem decrescente a partir do ValorVenda.
  select top 5
  p.Nome, p.ValorVenda from dbo.Produtos p
  order by ValorVenda DESC
  
-- Exclusao produtos da tabela “Loja.Produto” cujo preço de venda seja menor do que o valor de custo.
delete from dbo.Produtos
where ValorVenda < ValorCusto;