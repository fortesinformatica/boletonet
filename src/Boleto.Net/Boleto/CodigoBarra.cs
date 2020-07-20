using System;
using System.Drawing;
using System.Globalization;
using BoletoNet.Util;

namespace BoletoNet
{
    public class CodigoBarra
    {
        public CodigoBarra()
        {
            Chave = "";
            LinhaDigitavel = "";
            Imagem = null;
            Codigo = "";
            Moeda = 9;
        }

        /// <summary>
        /// Código de Barra
        /// </summary>
        public string Codigo { get; set; }

        public Image Imagem { get; private set; }

        /// <summary>
        /// Retorna a representação numérica do código de barra
        /// </summary>
        public string LinhaDigitavel { get; set; }

        /// <summary>
        /// Chave para montar Codigo de Barra
        /// </summary>
        public string Chave { get; set; }

        public string CodigoBanco { get; set; }

        public int Moeda { get; set; }
        
        public string CampoLivre { get; set; }
        
        public long FatorVencimento { get; set; }
        
        public string ValorDocumento { get; set; }

        public string DigitoVerificador
        {
            get
            {
                var codigoSemDv = $"{CodigoBanco}{Moeda}{FatorVencimento}{ValorDocumento}{CampoLivre}";
                int pesoMaximo = 9, soma = 0, peso = 2;

                for (var i = (codigoSemDv.Length - 1); i >= 0; i--)
                {
                    soma = soma + (Convert.ToInt32(codigoSemDv.Substring(i, 1)) * peso);
                    if (peso == pesoMaximo)
                        peso = 2;
                    else
                        peso = peso + 1;
                }

                var resto = (soma % 11);

                if (resto <= 1 || resto > 9)
                    return "1";

                return (11 - resto).ToString();
            }
        }

        public string LinhaDigitavelFormatada
        {
            get
            {
                var pt1 = (CodigoBanco + Moeda).PadRight(9, '0');
                var mod10 = AbstractBanco.Mod10(pt1);
                pt1 = (pt1 + mod10).Insert(5, ".");

                var substring = CampoLivre.Substring(5);

                var pt2 = substring.Substring(0, 10);
                mod10 = AbstractBanco.Mod10(pt2);
                pt2 = (pt2 + mod10).Insert(5, ".");

                var pt3 = substring.Substring(10);
                mod10 = AbstractBanco.Mod10(pt3);
                pt3 = (pt3 + mod10).Insert(5, ".");

                var pt5 = FatorVencimento + ValorDocumento;
                return string.Join(" ", new[] { pt1, pt2, pt3, DigitoVerificador, pt5 });
            }
        }

        public void PreencheValores(int codigoBanco, int moeda, long fatorVencimento, string valorDocumento, string campoLivre)
        {
            CodigoBanco = Utils.FormatCode(codigoBanco.ToString(), 3);
            Moeda = moeda;
            FatorVencimento = fatorVencimento;
            ValorDocumento = valorDocumento;
            CampoLivre = campoLivre;

            Codigo = string.Format("{0}{1}{2}{3}{4}",
                CodigoBanco,
                Moeda,
                FatorVencimento,
                ValorDocumento,
                CampoLivre);
        }
    }
}
