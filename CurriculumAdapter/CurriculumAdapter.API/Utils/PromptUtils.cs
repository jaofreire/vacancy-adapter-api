namespace CurriculumAdapter.API.Utils
{
    public class InputPrompt
    {
        public string PreRequisites { get; set; } 
        public string UserPrompt {  get; set; }

        public InputPrompt(string preRequisites, string userPrompt)
        {
            PreRequisites = preRequisites;
            UserPrompt = userPrompt;
        }

    }

    public static class PromptUtils
    {
        public static InputPrompt GenerateCurriculumAdapterPrompt(string description, string userSkills = "")
        {
            string preRequisites = @"
Você receberá três inputs, 2 obrigatórios e 1 opcional: 1. Um texto com a descrição da vaga. 2. Um arquivo com o currículo atual do cliente. 3(Opcional). Um texto com todas as competências do cliente. Sua tarefa é: Ler e entender o currículo e a vaga. Extrair todos os dados relevantes do currículo. Adaptar o currículo para a vaga, ajustando linguagem, foco e ênfase, caso o Texto de Competências não esteja vazio, altere o currículo de acordo com o texto, para evitar adicionar competências que o usuário não tenha, caso o Texto de Competências esteja vazio não leve em consideração o Texto de Competências. Retornar apenas o conteúdo formatado do currículo, pronto para ser convertido em PDF, em seguida retorne todos as alterações feitas e explicando de forma resumida o porque de cada alteração, caso o Texto de Competências não estiver vazio e a descrição da vaga enviada tiver habilidades/competências que o cliente não tem, caso Texto de Competências esteja vazio não precisa enviar as habilidades/competências não adicionadas, retorne estas competências na explicação das alterações, explicando que o cliente não tem as competências pedidas, Exemplo('Foi identificado que a descrição da vaga pede habilidades/competências que você não tem de acordo com o Texto de Competências: - Mensageria, - Kafka e/ou RabbitMq. Estas habilidades/competências não foram adicionadas no seu currículo') , para separar o curriculo alterado e os detalhes das alterações, adicione um @#$% separando essas duas informações. Formatação obrigatória: Negrito (Bold) no início de cada categoria e subcategoria: use apenas *texto* (asterisco simples) para marcar onde deve ser negrito. Exemplo: *Nome:* João da Silva. Itens de listas devem começar com um '-' (hífen + espaço). Não utilize nenhum outro caractere especial (como #, _, =,【, etc.). Não pule linhas entre conteúdos de uma mesma seção. Organize o texto de forma limpa e bonita, respeitando a hierarquia visual, não adicione observações ou avisos ou qualquer outro comentário no final ou inicio do texto que não envolva o conteúdo do currículo, Padrões obrigatórios: Não adicione comentários, explicações ou observações. Retorne apenas o conteúdo. Mantenha todas as informações originais do currículo, adaptadas para a vaga. Garanta que a estrutura final seja compatível com sistemas ATS (como Gupy, Glassdoor, Indeed etc.), ou seja: Linguagem direta e objetiva. Nada de emojis, tabelas ou colunas. Foco em palavras-chave relevantes para a vaga.";

            string userPrompt = $"Descrição: {description}. Texto de competências: {userSkills}";

            return new InputPrompt(preRequisites, userPrompt);
        }
    }
}
