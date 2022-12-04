namespace Organizacja
{
    public partial class Form1 : Form
    {
        public bool zapisac = false;
        public Form1()
        {

            InitializeComponent();
            OdczytZPliku();
        }

        private void contextMenuStrip1_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (zapisac)
            {
                DialogResult res = MessageBox.Show("Zapisaæ zmiany przed zamkniêciem?", "Zamykanie", MessageBoxButtons.YesNoCancel);
                if (res == DialogResult.Yes)
                {
                    ZapisDoPliku();
                    Application.Exit();
                }
                else if (res == DialogResult.No)
                {
                    Application.Exit();
                }
            }
            else
            {
                Close();
            }
        }

        private void fToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 dialog = new Form2();
            dialog.Text = "Dodawanie ga³êzi";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                treeView1.Nodes.Add(dialog.nazwa);
            }
        }

        private void dToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zapisac = true;
            Form2 dialog = new Form2();
            dialog.Text = "Dodawanie ga³êzi";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                treeView1.SelectedNode.Nodes.Add(dialog.nazwa);
            }
        }

        private void zmieñToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zapisac = true;
            Form2 dialog = new Form2();
            dialog.Text = "Dodawanie ga³êzi";

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                treeView1.SelectedNode.Text = dialog.nazwa;
            }
        }

        private void eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            zapisac = true;
            treeView1.Nodes.Remove(treeView1.SelectedNode);
        }

        private void DodajDoListy(TreeNode node, ref List<myNode> lista)
        {
            zapisac = true;
            if (node == null)
                return;
            string r, n;
            if (node.Parent == null)
                r = "brak";
            else
                r = node.Parent.Text;
            n = node.Text;

            lista.Add(new myNode(r, n));
            if (node.NextNode != null)
                DodajDoListy(node.NextNode, ref lista);
            if (node.GetNodeCount(true) > 0)
                DodajDoListy(node.FirstNode, ref lista);
        }

        private void ZapisDoPliku()
        {
            List<myNode> lista = new List<myNode>();



            if (treeView1.Nodes.Count == 0)
            {

                File.WriteAllText("firma.txt", "");
                return;
            }
            DodajDoListy(treeView1.Nodes[0], ref lista);

            string text = "";
            foreach (myNode elem in lista)
            {
                text += elem.nazwa + " " + elem.rodzic + "\n";
            }
            File.WriteAllText("firma.txt", text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ZapisDoPliku();
            Application.Exit();
        }

        private void OdczytZPliku()
        {
            treeView1.Nodes.Clear();
            List<myNode> lista = new List<myNode>();
            if (!(File.Exists("firma.txt")))
            {
                File.Create("firma.txt");

            }
            else
            {
                string[] tab = File.ReadAllLines("firma.txt");
                foreach (string elem in tab)
                {
                    string[] pom = elem.Split(' ');
                    lista.Add(new myNode(pom[1], pom[0]));
                }
                foreach (myNode node in lista)
                {
                    if (node.rodzic == "brak")
                    {
                        treeView1.Nodes.Add(new TreeNode(node.nazwa));
                    }
                    else
                    {
                        TreeNode rodzic = ZnajdzRodzica(treeView1.Nodes[0], node.rodzic);
                        if (rodzic != null)
                            rodzic.Nodes.Add(node.nazwa);
                    }
                }
            }
        }

        private TreeNode ZnajdzRodzica(TreeNode node, string rodzic)

        {

            TreeNode wynik = null;

            if (node == null)
                return null;

            if (node.Text == rodzic)
                return node;

            if (node.NextNode != null)
                wynik = ZnajdzRodzica(node.NextNode, rodzic);

            if (node.GetNodeCount(true) > 0)
                wynik = ZnajdzRodzica(node.FirstNode, rodzic);

            return wynik;

        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void contextMenuStrip1_Opening_1(object sender, System.ComponentModel.CancelEventArgs e)
        {

        }
    }

    public class myNode
    {
        public string rodzic;
        public string nazwa;

        public myNode(string rodzic, string nazwa)
        {
            this.rodzic = rodzic;
            this.nazwa = nazwa;
        }
    }
}