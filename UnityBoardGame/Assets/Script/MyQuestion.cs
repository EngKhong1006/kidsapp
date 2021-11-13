[System.Serializable]
public class MyQuestion
{
    public string question;
    public string[] choice;
    public int ansIndex;

    public MyQuestion(MyQuestion myQuestion)
    {
        this.question = myQuestion.question;
        this.choice = myQuestion.choice;
        this.ansIndex = myQuestion.ansIndex;
    }
    public bool checkAns(int selected_ans)
    {
        return selected_ans == this.ansIndex;
    }
}
