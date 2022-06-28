namespace DiscordTemplate
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        // Start
        private void button1_Click(object sender, EventArgs e)
        {

        }

        // Test connection
        private void button2_Click(object sender, EventArgs e)
        {
            var testcon = new Connection.Connect();
            testcon.Connection();
        }
    }
}