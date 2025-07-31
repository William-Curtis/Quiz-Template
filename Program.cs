namespace Quiz
{
    class Quiz
    {
        private List<Question> questions;
        private double score;
        private double maxScore;
        private bool Feedback = false;
        private bool RandomizeQuestions = false;
        private bool RandomizeOptions = false;
        private string addDescription = "";
        private string topic = "";
        public Quiz()
        {
            this.questions = new List<Question>();
        }
        public void setTopic(string topic)
        {
            this.topic = topic;
        }
        public string getTopic()
        {
            return this.topic;
        }
        public void setFeedback(bool feedback)
        {
            this.Feedback = feedback;
        }
        public void setAdditionalDescription(string text)
        {
            this.addDescription = text;
        }
        public void setRandomizeQuestions(bool randomize)
        {
            this.RandomizeQuestions = randomize;
        }
        public void setRandomizeOptions(bool randomize)
        {
            this.RandomizeOptions = randomize;
        }
        public void addQuestion(Question question)
        {
            this.questions.Add(question);
        }
        public void removeQuestion(int idx)
        {
            this.questions.RemoveAt(idx);
        }
        public Question getQuestion(int idx)
        {
            return this.questions.ElementAt(idx);
        }
        public void runQuestion(Question q)
        {
            bool loop;
            string userInput;
            int answer = -1;
            bool[] tmpAnswer;
            Console.WriteLine(q.ToString());
            do
            {
                loop = false;
                Console.Write("Enter your Answer: ");
                userInput = Console.ReadLine();
                tmpAnswer = new bool[q.getOptions().Count];
                if (q is MultipleChoiceQuestion)
                {
                    if (userInput.Length != 1)
                    {
                        loop = true;
                        Console.WriteLine("Invalid Input.");
                    }
                    else
                    {
                        answer = userInput[0] - 97;
                        if (answer < q.getOptions().Count && answer >= 0)
                        {
                            this.maxScore += q.getScore();
                            if (((MultipleChoiceQuestion)q).checkAnswer(answer))
                            {
                                this.score += q.getScore();
                            }
                        }
                        else
                        {
                            loop = true;
                            Console.WriteLine("Invalid Input.");
                        }
                    }
                }
                else if (q is MultipleResponseQuestion)
                {
                    for (int i = 0; i < tmpAnswer.Length; i++)
                    {
                        tmpAnswer[i] = false;
                    }
                    if (userInput.Length == 0)
                    {
                        loop = true;
                        Console.WriteLine("Invalid Input.");
                    }
                    for (int i = 0; i < userInput.Length && !loop; i++)
                    {
                        if (!(userInput[i] == ' ' || userInput[i] == ','))
                        {
                            answer = userInput[i] - 97;
                            if (answer < q.getOptions().Count && answer >= 0)
                            {
                                tmpAnswer[answer] = true;
                            }
                            else
                            {
                                loop = true;
                                Console.WriteLine("Invalid Input.");
                            }
                        }
                    }
                    if (!loop)
                    {
                        this.score += ((MultipleResponseQuestion)q).checkAnswer(tmpAnswer);
                        this.maxScore += q.getScore();
                    }
                }
            } while (loop);

            if (this.Feedback)
            {
                if (q is MultipleChoiceQuestion)
                {
                    if (((MultipleChoiceQuestion)q).checkAnswer(answer))
                    {
                        Console.WriteLine("Correct! The answer is " + ((char)(((MultipleChoiceQuestion)q).getAnswer() + 97)) + ".\n");
                    }
                    else
                    {
                        Console.WriteLine("Incorrect. The answer is " + ((char)(((MultipleChoiceQuestion)q).getAnswer() + 97)) + ".\n");
                    }
                }
                else if (q is MultipleResponseQuestion)
                {
                    if (((MultipleResponseQuestion)q).checkAnswer(tmpAnswer) == q.getScore())
                    {
                        Console.WriteLine("Correct!");
                    }
                    else if (((MultipleResponseQuestion)q).checkAnswer(tmpAnswer) != 0)
                    {
                        Console.WriteLine("Partially Correct.");
                    }
                    else
                    {
                        Console.WriteLine("Incorrect.");
                    }
                    Console.WriteLine("The correct answers are: " + ((MultipleResponseQuestion)q).answerToString() + "\n");
                }
            }
        }
        public void start()
        {
            this.score = 0;
            this.maxScore = 0;
            Console.WriteLine("\n" + this.topic.ToUpper() + " QUIZ\n");
            Console.WriteLine("This quiz will test your understanding of " + this.topic + ".");
            Console.WriteLine("You will be presented with " + this.questions.Count + " questions.");
            Console.WriteLine("For multi-select questions, partial marks are given for partially correct answers.");
            if (!this.addDescription.Equals(""))
            {
                Console.WriteLine(this.addDescription);
            }
            Console.WriteLine("Read each question carefully before making your selection.");
            if (this.Feedback)
            {
                Console.WriteLine("Once you have selected your answer(s), you will be provided with immediate feedback.");
            }
            Console.WriteLine("Your overall score will be displayed at the end of the quiz.\n");
            if (this.RandomizeQuestions)
            {
                int n = this.questions.Count;
                Random rng = new Random();
                while (n > 1)
                {
                    n--;
                    int k = rng.Next(n + 1);
                    Question value = this.questions[k];
                    this.questions[k] = this.questions[n];
                    this.questions[n] = value;
                }
            }
            if (this.RandomizeOptions)
            {
                Random rng = new Random();
                for (int i = 0; i < this.questions.Count; i++)
                {
                    int n = this.questions[i].getOptions().Count;
                    while (n > 1)
                    {
                        rng = new Random();
                        n--;
                        int k = rng.Next(n + 1);
                        if (this.questions[i] is MultipleChoiceQuestion)
                        {
                            if (((MultipleChoiceQuestion)this.questions[i]).getAnswer() == k) {
                                ((MultipleChoiceQuestion)this.questions[i]).setAnswer(n);
                            }
                            else if (((MultipleChoiceQuestion)this.questions[i]).getAnswer() == n) {
                                ((MultipleChoiceQuestion)this.questions[i]).setAnswer(k);
                            }
                        }
                        else if (this.questions[i] is MultipleResponseQuestion)
                        {
                            if (((MultipleResponseQuestion)this.questions[i]).getAnswers().Contains(k))
                            {
                                if (!((MultipleResponseQuestion)this.questions[i]).getAnswers().Contains(n))
                                {
                                    ((MultipleResponseQuestion)this.questions[i]).getAnswers().Remove(k);
                                    ((MultipleResponseQuestion)this.questions[i]).getAnswers().Add(n);
                                }
                            }
                            else if (((MultipleResponseQuestion)this.questions[i]).getAnswers().Contains(n))
                            {
                                ((MultipleResponseQuestion)this.questions[i]).getAnswers().Remove(n);
                                ((MultipleResponseQuestion)this.questions[i]).getAnswers().Add(k);
                            }
                        }
                        string value = this.questions[i].getOptions()[k];
                        this.questions[i].getOptions()[k] = this.questions[i].getOptions()[n];
                        this.questions[i].getOptions()[n] = value;
                    }
                }
            }
            for (int activeQuestion = 0; activeQuestion < this.questions.Count; activeQuestion++)
            {
                Console.WriteLine("Question " + (activeQuestion + 1) + " of " + this.questions.Count);
                runQuestion(questions.ElementAt(activeQuestion));
                if (this.Feedback)
                {
                    if (activeQuestion != this.questions.Count - 1)
                    {
                        Console.WriteLine("Press any key to continue.");
                    }
                    else
                    {
                        Console.WriteLine("Press any key to view your overall quiz results.");
                    }
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine("Your final score is " + Math.Round((this.score / this.maxScore) * 100) + "% (" + Math.Round(this.score * 100000) / 100000 + " / " + this.maxScore + " points)\n");
            Console.ReadLine();
        }
    }

    abstract class Question
    {
        private static readonly int MAX_OPTIONS = 10;
        private double score;
        private string question = "";
        private List<string> options = new List<string>();
        public void setQuestion(string question)
        {
            this.question = question;
        }
        public string getQuestion()
        {
            return this.question;
        }
        public void setScore(double score)
        {
            this.score = score;
        }
        public double getScore()
        {
            return this.score;
        }
        public void setOptions(List<string> answers)
        {
            if (answers.Count > MAX_OPTIONS)
            {
                throw new ArgumentOutOfRangeException();
            }
            this.options = answers;
        }
        public List<string> getOptions()
        {
            return this.options;
        }
        public string getOptionsAsString()
        {
            string result = "";
            for (int i = 0; i < getOptions().Count; i++)
            {
                result += "(" + ((char)(i + 97)) + ") " + getOptions().ElementAt(i) + "\n";
            }
            return result;
        }
        public override string ToString()
        {
            return question + "\n";
        }
    }

    class MultipleChoiceQuestion : Question
    {
        private int answer;
        public MultipleChoiceQuestion(string question, List<string> options, int answer, double score = 1)
        {
            setQuestion(question);
            setOptions(options);
            this.answer = answer;
            setScore(score);
        }
        public bool checkAnswer(int answer)
        {
            return this.answer == answer;
        }
        public int getAnswer()
        {
            return this.answer;
        }
        public void setAnswer(int answer)
        {
            this.answer = answer;
        }
        public override string ToString()
        {
            return base.ToString() + "Select 1 of the " + base.getOptions().Count + " options:\n\n" + getOptionsAsString();
        }
    }

    class MultipleResponseQuestion : Question
    {
        private List<int> answers;
        public MultipleResponseQuestion(string question, List<string> options, List<int> answers, double score = 1)
        {
            setQuestion(question);
            setOptions(options);
            this.answers = answers;
            setScore(score);
        }
        public double checkAnswer(bool[] answers)
        {
            double tmpScore = 0;
            for (int i = 0; i < answers.Length; i++)
            {
                if (answers[i] && this.answers.Contains(i))
                {
                    tmpScore += 1;
                }
                else if (!answers[i] && !this.answers.Contains(i))
                {
                    tmpScore += 1;
                }
            }
            return tmpScore / (base.getScore() * this.getOptions().Count);
        }
        public List<int> getAnswers()
        {
            return this.answers;
        }
        public String answerToString()
        {
            answers.Sort();
            if (this.answers.Count == 1)
            {
                return "" + (char)(answers.ElementAt(0) + 97);
            }
            else if (this.answers.Count == 2)
            {
                return (char)(answers.ElementAt(0) + 97) + " and " + (char)(answers.ElementAt(1) + 97);
            }
            string result = "";
            for (int i = 0; i < this.answers.Count; i++)
            {
                result += (char)(answers.ElementAt(i) + 97);
                if (i == this.answers.Count - 2)
                {
                    result += ", and ";
                }
                else if (i != this.answers.Count - 1)
                {
                    result += ", ";
                }
            }
            return result;
        }
        public override string ToString()
        {
            return base.ToString() + "Select all that apply from the " + base.getOptions().Count + " options:\n\n" + getOptionsAsString();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Quiz quiz = new Quiz();
            quiz.setTopic("Lorem Ipsum");

            //Q1
            List<string> optionsQ1 = new List<string>();
            optionsQ1.Add("Option 1");
            optionsQ1.Add("Option 2");
            optionsQ1.Add("Option 3");
            optionsQ1.Add("Option 4");
            quiz.addQuestion(new MultipleChoiceQuestion(quiz.getTopic() + " Question Stem 1.", optionsQ1, 1));

            //Q2
            List<string> optionsQ2 = new List<string>();
            optionsQ2.Add("Option 1");
            optionsQ2.Add("Option 2");
            optionsQ2.Add("Option 3");
            optionsQ2.Add("Option 4");
            quiz.addQuestion(new MultipleChoiceQuestion(quiz.getTopic() + " Question Stem 2.", optionsQ2, 1));

            //Q3
            List<string> optionsQ3 = new List<string>();
            optionsQ3.Add("Option 1");
            optionsQ3.Add("Option 2");
            optionsQ3.Add("Option 3");
            optionsQ3.Add("Option 4");
            quiz.addQuestion(new MultipleChoiceQuestion(quiz.getTopic() + " Question Stem 3.", optionsQ3, 1));

            //Q4
            List<string> optionsQ4 = new List<string>();
            optionsQ4.Add("Option 1");
            optionsQ4.Add("Option 2");
            optionsQ4.Add("Option 3");
            optionsQ4.Add("Option 4");
            quiz.addQuestion(new MultipleChoiceQuestion(quiz.getTopic() + " Question Stem 4.", optionsQ4, 1));

            //Q5
            List<string> optionsQ5 = new List<string>();
            optionsQ5.Add("Option 1");
            optionsQ5.Add("Option 2");
            optionsQ5.Add("Option 3");
            optionsQ5.Add("Option 4");
            quiz.addQuestion(new MultipleChoiceQuestion(quiz.getTopic() + " Question Stem 5.", optionsQ5, 1));

            //Q6
            List<string> optionsQ6 = new List<string>();
            List<int> answersQ6 = new List<int>();
            optionsQ6.Add("Option 1");
            optionsQ6.Add("Option 2");
            optionsQ6.Add("Option 3");
            optionsQ6.Add("Option 4");
            answersQ6.Add(0);
            answersQ6.Add(1);
            answersQ6.Add(2);
            quiz.addQuestion(new MultipleResponseQuestion(quiz.getTopic() + " Question Stem 6.", optionsQ6, answersQ6));

            //Q7
            List<string> optionsQ7 = new List<string>();
            List<int> answersQ7 = new List<int>();
            optionsQ7.Add("Option 1");
            optionsQ7.Add("Option 2");
            optionsQ7.Add("Option 3");
            optionsQ7.Add("Option 4");
            answersQ7.Add(0);
            answersQ7.Add(1);
            answersQ7.Add(2);
            quiz.addQuestion(new MultipleResponseQuestion(quiz.getTopic() + " Question Stem 7.", optionsQ7, answersQ7));

            //Q8
            List<string> optionsQ8 = new List<string>();
            List<int> answersQ8 = new List<int>();
            optionsQ8.Add("Option 1");
            optionsQ8.Add("Option 2");
            optionsQ8.Add("Option 3");
            optionsQ8.Add("Option 4");
            answersQ8.Add(0);
            answersQ8.Add(1);
            answersQ8.Add(2);
            quiz.addQuestion(new MultipleResponseQuestion(quiz.getTopic() + " Question Stem 8.", optionsQ8, answersQ8));

            //Q9
            List<string> optionsQ9 = new List<string>();
            List<int> answersQ9 = new List<int>();
            optionsQ9.Add("Option 1");
            optionsQ9.Add("Option 2");
            optionsQ9.Add("Option 3");
            optionsQ9.Add("Option 4");
            answersQ9.Add(0);
            answersQ9.Add(1);
            answersQ9.Add(2);
            quiz.addQuestion(new MultipleResponseQuestion(quiz.getTopic() + " Question Stem 9.", optionsQ9, answersQ9));

            //Q10
            List<string> optionsQ10 = new List<string>();
            List<int> answersQ10 = new List<int>();
            optionsQ10.Add("Option 1");
            optionsQ10.Add("Option 2");
            optionsQ10.Add("Option 3");
            optionsQ10.Add("Option 4");
            answersQ10.Add(0);
            answersQ10.Add(1);
            answersQ10.Add(2);
            quiz.addQuestion(new MultipleResponseQuestion(quiz.getTopic() + " Question Stem 10.", optionsQ10, answersQ10));

            // Settings
            quiz.setFeedback(true);
            quiz.setRandomizeQuestions(true);
            quiz.setRandomizeOptions(true);
            // quiz.setAdditionalDescription("");

            // Start Quiz
            quiz.start();
        }
    }
}
