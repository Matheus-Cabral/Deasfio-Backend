using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.Json;

namespace DesafioBackend
{
    // Models for Sales (Desafio 1)
    public class Venda
    {
        public string Vendedor { get; set; } = string.Empty;
        public decimal Valor { get; set; }
    }

    public class VendasWrapper
    {
        public List<Venda> Vendas { get; set; } = new List<Venda>();
    }

    // Models for Stock (Desafio 2)
    public class ProdutoEstoque
    {
        public int CodigoProduto { get; set; }
        public string DescricaoProduto { get; set; } = string.Empty;
        public int Estoque { get; set; }
    }

    public class EstoqueWrapper
    {
        public List<ProdutoEstoque> Estoque { get; set; } = new List<ProdutoEstoque>();
    }

    public class MovimentacaoEstoque
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int CodigoProduto { get; set; }
        public string Descricao { get; set; } = string.Empty;
        public int Quantidade { get; set; }
        public bool EhEntrada { get; set; }
        public DateTime Data { get; set; } = DateTime.Now;
    }

    internal class Program
    {
        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("pt-BR");

            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Desafio Backend ===");
                Console.WriteLine("1 - Calcular comissão de vendedores (Desafio 1)");
                Console.WriteLine("2 - Movimentar estoque (Desafio 2)");
                Console.WriteLine("3 - Calcular juros por atraso (Desafio 3)");
                Console.WriteLine("0 - Sair");
                Console.Write("Escolha uma opção: ");

                var opcao = Console.ReadLine();

                switch (opcao)
                {
                    case "1":
                        ExecutarDesafio1();
                        break;
                    case "2":
                        ExecutarDesafio2();
                        break;
                    case "3":
                        ExecutarDesafio3();
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Opção inválida.");
                        Pausar();
                        break;
                }
            }
        }

        #region Desafio 1 - Comissão de vendas

        private static void ExecutarDesafio1()
        {
            Console.Clear();
            Console.WriteLine("=== Desafio 1 - Comissão de Vendedores ===");

            var json = ObterJsonVendas();
            var wrapper = JsonSerializer.Deserialize<VendasWrapper>(json);

            if (wrapper == null || wrapper.Vendas == null || wrapper.Vendas.Count == 0)
            {
                Console.WriteLine("Não foi possível carregar os dados de vendas.");
                Pausar();
                return;
            }

            // Regra:
            // < 100,00 -> 0%
            // < 500,00 -> 1%
            // >= 500,00 -> 5%

            var comissoesPorVendedor = wrapper.Vendas
                .GroupBy(v => v.Vendedor)
                .Select(g => new
                {
                    Vendedor = g.Key,
                    TotalVendido = g.Sum(v => v.Valor),
                    TotalComissao = g.Sum(v => CalcularComissaoVenda(v.Valor))
                })
                .OrderBy(r => r.Vendedor)
                .ToList();

            Console.WriteLine();
            foreach (var item in comissoesPorVendedor)
            {
                Console.WriteLine($"Vendedor: {item.Vendedor}");
                Console.WriteLine($"  Total vendido: R$ {item.TotalVendido:N2}");
                Console.WriteLine($"  Comissão total: R$ {item.TotalComissao:N2}");
                Console.WriteLine();
            }

            Pausar();
        }

        private static decimal CalcularComissaoVenda(decimal valor)
        {
            if (valor < 100m)
                return 0m;
            if (valor < 500m)
                return valor * 0.01m; // 1%
            return valor * 0.05m;    // 5%
        }

        private static string ObterJsonVendas()
        {
            // JSON fornecido no enunciado (mantido como string para simplificar o desafio)
            return @"
{
  ""vendas"": [
    { ""vendedor"": ""João Silva"", ""valor"": 1200.50 },
    { ""vendedor"": ""João Silva"", ""valor"": 950.75 },
    { ""vendedor"": ""João Silva"", ""valor"": 1800.00 },
    { ""vendedor"": ""João Silva"", ""valor"": 1400.30 },
    { ""vendedor"": ""João Silva"", ""valor"": 1100.90 },
    { ""vendedor"": ""João Silva"", ""valor"": 1550.00 },
    { ""vendedor"": ""João Silva"", ""valor"": 1700.80 },
    { ""vendedor"": ""João Silva"", ""valor"": 250.30 },
    { ""vendedor"": ""João Silva"", ""valor"": 480.75 },
    { ""vendedor"": ""João Silva"", ""valor"": 320.40 },

    { ""vendedor"": ""Maria Souza"", ""valor"": 2100.40 },
    { ""vendedor"": ""Maria Souza"", ""valor"": 1350.60 },
    { ""vendedor"": ""Maria Souza"", ""valor"": 950.20 },
    { ""vendedor"": ""Maria Souza"", ""valor"": 1600.75 },
    { ""vendedor"": ""Maria Souza"", ""valor"": 1750.00 },
    { ""vendedor"": ""Maria Souza"", ""valor"": 1450.90 },
    { ""vendedor"": ""Maria Souza"", ""valor"": 400.50 },
    { ""vendedor"": ""Maria Souza"", ""valor"": 180.20 },
    { ""vendedor"": ""Maria Souza"", ""valor"": 90.75 },

    { ""vendedor"": ""Carlos Oliveira"", ""valor"": 800.50 },
    { ""vendedor"": ""Carlos Oliveira"", ""valor"": 1200.00 },
    { ""vendedor"": ""Carlos Oliveira"", ""valor"": 1950.30 },
    { ""vendedor"": ""Carlos Oliveira"", ""valor"": 1750.80 },
    { ""vendedor"": ""Carlos Oliveira"", ""valor"": 1300.60 },
    { ""vendedor"": ""Carlos Oliveira"", ""valor"": 300.40 },
    { ""vendedor"": ""Carlos Oliveira"", ""valor"": 500.00 },
    { ""vendedor"": ""Carlos Oliveira"", ""valor"": 125.75 },

    { ""vendedor"": ""Ana Lima"", ""valor"": 1000.00 },
    { ""vendedor"": ""Ana Lima"", ""valor"": 1100.50 },
    { ""vendedor"": ""Ana Lima"", ""valor"": 1250.75 },
    { ""vendedor"": ""Ana Lima"", ""valor"": 1400.20 },
    { ""vendedor"": ""Ana Lima"", ""valor"": 1550.90 },
    { ""vendedor"": ""Ana Lima"", ""valor"": 1650.00 },
    { ""vendedor"": ""Ana Lima"", ""valor"": 75.30 },
    { ""vendedor"": ""Ana Lima"", ""valor"": 420.90 },
    { ""vendedor"": ""Ana Lima"", ""valor"": 315.40 }
  ]
}";
        }

        #endregion

        #region Desafio 2 - Movimentação de estoque

        private static void ExecutarDesafio2()
        {
            Console.Clear();
            Console.WriteLine("=== Desafio 2 - Movimentação de Estoque ===");

            var json = ObterJsonEstoque();
            var wrapper = JsonSerializer.Deserialize<EstoqueWrapper>(json);

            if (wrapper == null || wrapper.Estoque == null || wrapper.Estoque.Count == 0)
            {
                Console.WriteLine("Não foi possível carregar o estoque.");
                Pausar();
                return;
            }

            var estoque = wrapper.Estoque;
            var movimentacoes = new List<MovimentacaoEstoque>();

            while (true)
            {
                Console.Clear();
                Console.WriteLine("Produtos em estoque:");
                foreach (var p in estoque)
                    Console.WriteLine($"Código: {p.CodigoProduto} | Descrição: {p.DescricaoProduto} | Estoque atual: {p.Estoque}");

                Console.WriteLine();
                Console.WriteLine("1 - Lançar movimentação");
                Console.WriteLine("0 - Voltar ao menu principal");
                Console.Write("Escolha: ");
                var opcao = Console.ReadLine();

                if (opcao == "0")
                    break;

                if (opcao != "1")
                {
                    Console.WriteLine("Opção inválida.");
                    Pausar();
                    continue;
                }

                Console.Write("Informe o código do produto: ");
                if (!int.TryParse(Console.ReadLine(), out int codigo))
                {
                    Console.WriteLine("Código inválido.");
                    Pausar();
                    continue;
                }

                var produto = estoque.FirstOrDefault(p => p.CodigoProduto == codigo);
                if (produto == null)
                {
                    Console.WriteLine("Produto não encontrado.");
                    Pausar();
                    continue;
                }

                Console.Write("Tipo de movimentação (E = Entrada, S = Saída): ");
                var tipo = Console.ReadLine()?.Trim().ToUpperInvariant();
                bool ehEntrada;

                if (tipo == "E")
                    ehEntrada = true;
                else if (tipo == "S")
                    ehEntrada = false;
                else
                {
                    Console.WriteLine("Tipo inválido.");
                    Pausar();
                    continue;
                }

                Console.Write("Quantidade: ");
                if (!int.TryParse(Console.ReadLine(), out int quantidade) || quantidade <= 0)
                {
                    Console.WriteLine("Quantidade inválida.");
                    Pausar();
                    continue;
                }

                if (!ehEntrada && quantidade > produto.Estoque)
                {
                    Console.WriteLine("Não há estoque suficiente para essa saída.");
                    Pausar();
                    continue;
                }

                Console.Write("Descrição da movimentação: ");
                var descricao = Console.ReadLine() ?? string.Empty;

                var mov = new MovimentacaoEstoque
                {
                    CodigoProduto = produto.CodigoProduto,
                    Descricao = descricao,
                    Quantidade = quantidade,
                    EhEntrada = ehEntrada
                };

                movimentacoes.Add(mov);

                if (ehEntrada)
                    produto.Estoque += quantidade;
                else
                    produto.Estoque -= quantidade;

                Console.WriteLine();
                Console.WriteLine($"Movimentação registrada com ID: {mov.Id}");
                Console.WriteLine($"Estoque final do produto '{produto.DescricaoProduto}' (cód. {produto.CodigoProduto}): {produto.Estoque}");
                Pausar();
            }
        }

        private static string ObterJsonEstoque()
        {
            return @"
{
  ""estoque"": [
    {
      ""codigoProduto"": 101,
      ""descricaoProduto"": ""Caneta Azul"",
      ""estoque"": 150
    },
    {
      ""codigoProduto"": 102,
      ""descricaoProduto"": ""Caderno Universitário"",
      ""estoque"": 75
    },
    {
      ""codigoProduto"": 103,
      ""descricaoProduto"": ""Borracha Branca"",
      ""estoque"": 200
    },
    {
      ""codigoProduto"": 104,
      ""descricaoProduto"": ""Lápis Preto HB"",
      ""estoque"": 320
    },
    {
      ""codigoProduto"": 105,
      ""descricaoProduto"": ""Marcador de Texto Amarelo"",
      ""estoque"": 90
    }
  ]
}";
        }

        #endregion

        #region Desafio 3 - Juros por atraso

        private static void ExecutarDesafio3()
        {
            Console.Clear();
            Console.WriteLine("=== Desafio 3 - Cálculo de Juros por Atraso ===");
            Console.WriteLine("Multa: 2,5% ao dia sobre o valor em atraso.");
            Console.WriteLine();

            Console.Write("Informe o valor original (R$): ");
            if (!decimal.TryParse(Console.ReadLine(), out decimal valorOriginal) || valorOriginal <= 0)
            {
                Console.WriteLine("Valor inválido.");
                Pausar();
                return;
            }

            Console.Write("Informe a data de vencimento (dd/MM/aaaa): ");
            if (!DateTime.TryParseExact(Console.ReadLine(), "dd/MM/yyyy", new CultureInfo("pt-BR"), DateTimeStyles.None, out DateTime dataVencimento))
            {
                Console.WriteLine("Data inválida.");
                Pausar();
                return;
            }

            var hoje = DateTime.Today;
            if (hoje <= dataVencimento)
            {
                Console.WriteLine();
                Console.WriteLine("Título não está em atraso. Juros: R$ 0,00");
                Console.WriteLine($"Valor final: R$ {valorOriginal:N2}");
                Pausar();
                return;
            }

            int diasAtraso = (hoje - dataVencimento).Days;
            const decimal taxaDiaria = 0.025m; // 2,5% ao dia

            decimal valorJuros = valorOriginal * taxaDiaria * diasAtraso;
            decimal valorFinal = valorOriginal + valorJuros;

            Console.WriteLine();
            Console.WriteLine($"Dias em atraso: {diasAtraso}");
            Console.WriteLine($"Juros total: R$ {valorJuros:N2}");
            Console.WriteLine($"Valor final com juros: R$ {valorFinal:N2}");
            Pausar();
        }

        #endregion

        private static void Pausar()
        {
            Console.WriteLine();
            Console.Write("Pressione ENTER para continuar...");
            Console.ReadLine();
        }
    }
}
