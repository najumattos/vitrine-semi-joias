# Desafios de Banco de Dados - SQL Server

## Como Usar

Para Rodar as Consultas
Para executar as consultas, utilize dois scripts:
- Criação das tabelas e inserção dos dados: *createDB.sql*

## Desafios Iniciante
* **Desafio 1 UPDATE:** Atualizar o preço de um produto
```sql
update dbo.Produtos
set ValorVenda = 15.00
where Nome = 'Café'
```
* **Desafio 2 SELECT:** Recuperar os cinco produtos mais caros ordenados em ordem decrescente
```sql
  select top 5
  p.Nome, p.ValorVenda from dbo.Produtos p
  order by ValorVenda DESC
```
* **Desafio DELET:** Excluir produtos cujo preço de venda é menor do que o valor de custo
```sql
delete from dbo.Produtos
where ValorVenda < ValorCusto;
```

## Desafios Intermediario
* **Desafio 1 Consulta de Produtos:** Recuperar Produto.nome_produto, ProdutoCategoria.titulo_categoria e ProdutoEstoque.quantidade_produto, utilizando INNER JOIN para unir as tabelas
```sql
SELECT  C.categoria  AS Categoria,
		P.nome		 AS Produto,
		E.quantidade AS Saldo
FROM dbo.Produtos P
	INNER JOIN dbo.ProdutoCategoria C ON C.id_categoria = P.categoria
	INNER JOIN dbo.ProdutoEstoque E ON E.id_produto_estoque = P.id_produto;
GO
```
* **Desafio 2 Excluir Produtos Por Categoria:** Excluir todos os produtos da categoria "Roupas"
```sql
--Excluir Produtos da categoria "ROUPAS"(1) da tabela ProdutoEstoque
DELETE FROM dbo.ProdutoEstoque
WHERE produto IN (
	SELECT id_produto
	FROM dbo.Produtos
	WHERE categoria = 1
)
--Excluir Produtos da categoria "ROUPAS"
DELETE FROM dbo.Produtos
WHERE  categoria = 1;		
```
* **Desafio 3: Concatenar Nome Completo com CONCAT e CASE:** Criar uma coluna *NomeCompleto* concatenando os campos *titulo_cliente, primeiro_nome, segundo_nome e sobrenome_cliente* da tabela **Clientes**. A consulta utiliza **CONCAT** e **CASE** para tratar valores *NULL*, garantindo que não haja espaços extras no resultado.
```sql
SELECT titulo		 AS Título,
	   primeiro_nome AS 'Primeiro Nome',
	   segundo_nome  AS 'Segundo Nome',
	   sobrenome	 AS 'Sobrenome',
	CASE
		WHEN titulo IS NULL
		THEN CONCAT(primeiro_nome, ' ', segundo_nome, ' ', sobrenome) 

		WHEN segundo_nome IS NULL
		THEN CONCAT(titulo, ' ', primeiro_nome, ' ', sobrenome) 

		ELSE CONCAT(titulo, ' ', primeiro_nome, ' ', segundo_nome, ' ', sobrenome) 
	END AS 'Nome Completo'
	FROM dbo.Clientes
```

## Desafios Extra
* **Função SaldoCategoria(2):** Retorna a quantidade total de produtos para uma categoria específica
```sql
SELECT dbo.SaldoCategoria(3) AS 'Saldo por Categoria'
```
> Saldo por Categoria: Total de 45 Calçados

* **Função SaldoProduto(2):** Retorna a quantidade total de um produto específico em estoque
```sql
SELECT dbo.SaldoProduto(15) AS 'Saldo por Produto'
```
> Saldo por Produto: 15 unidades de Blusa

