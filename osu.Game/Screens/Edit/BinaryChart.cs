public struct BinaryChart
{
    public string name;
    public float bpm;
    public int id;
    public int offset;
    public int startpos;
    public int hardStartpos;
    public int endpos;
    public int hardEndpos;
    public int audioId;
    public float difficulty;
    public int sampleRate;
    public MyVector2[] difficultyLine;
    public BinaryNote[] notes;
}
