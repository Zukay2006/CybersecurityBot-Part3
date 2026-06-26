namespace CyberSecurityBotPart2
{
    internal class QuizQuestion
    {
        public string QuestionText { get; set; }
        public string OptionA { get; set; }
        public string OptionB { get; set; }
        public string OptionC { get; set; }
        public string OptionD { get; set; }
        public string CorrectAnswer { get; set; }
        public string Explanation { get; set; }
        public bool IsTrueFalse { get; set; }

        public QuizQuestion(
            string questionText,
            string optionA,
            string optionB,
            string optionC,
            string optionD,
            string correctAnswer,
            string explanation,
            bool isTrueFalse = false)
        {
            QuestionText = questionText;
            OptionA = optionA;
            OptionB = optionB;
            OptionC = optionC;
            OptionD = optionD;
            CorrectAnswer = correctAnswer;
            Explanation = explanation;
            IsTrueFalse = isTrueFalse;
        }
    }
}