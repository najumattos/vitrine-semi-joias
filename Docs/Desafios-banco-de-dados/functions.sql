USE Estoque_DB;
GO

CREATE FUNCTION SaldoCategoria(
@cID AS NUMERIC(5))
RETURNS NVARCHAR(100)
AS
BEGIN
	IF @cID IS NULL RETURN 'Informe uma categoria...';

	DECLARE @NomeCategoria NVARCHAR(50);
	DECLARE @SaldoCategoria NUMERIC(5);	
	DECLARE @msgFinal NVARCHAR(100);

	--Relaciona o @cID com o nome da categoria
	SELECT @NomeCategoria = C.categoria	
	FROM dbo.ProdutoCategoria C
	WHERE C.id_categoria = @cID;

	--Soma todos os itens de acordo com a categoria @cID
	SELECT @SaldoCategoria = SUM(E.quantidade)
	FROM dbo.ProdutoEstoque E
	INNER JOIN Produtos P ON E.produto = P.id_produto
	WHERE  P.categoria = @cID

	SET @msgFinal = CONCAT('Total de ', @NomeCategoria, ': ', @SaldoCategoria);
	RETURN @msgFinal;
END
GO

CREATE FUNCTION SaldoProduto(@pID AS NUMERIC(5))
RETURNS NVARCHAR(100)
AS
BEGIN
	IF @pID IS NULL RETURN 'Produto (ID) não foi informado...';--nao funfa?

	DECLARE @Saldo NUMERIC(5);
	DECLARE @NomeProduto NVARCHAR(50);
	DECLARE @msgFinal NVARCHAR(100);

	SELECT @NomeProduto = nome
	FROM dbo.Produtos
	WHERE id_produto = @pID;

	SELECT @Saldo = quantidade
	FROM dbo.ProdutoEstoque
	where id_produto_estoque = @pID;

	--Retorna o Saldo
	SET @msgFinal = CONCAT(@Saldo, ' unidades de ', @NomeProduto);
	RETURN @msgFinal;
END
GO

--Função para retornar a quantidade total de produtos por categoria
SELECT dbo.SaldoCategoria(3) AS 'Saldo por Categoria'

--Função para retornar a quantidade total de um produto
SELECT dbo.SaldoProduto(15) AS 'Saldo por Produto'