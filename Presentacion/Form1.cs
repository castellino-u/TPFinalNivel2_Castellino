using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dominio;
using Negocio;


namespace Presentacion
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Text = "Catálogo";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ArticuloNegocio articuloNegocio = new ArticuloNegocio();
            List<Articulo> lista = new List<Articulo>();
            lista = articuloNegocio.listar();
            dgvArticulo.DataSource = lista;
            cargarImagen(lista[0].ImagenUrl);
        }

        private void pbxArticulo_Click(object sender, EventArgs e)
        {

        }
        public void cargarImagen(string imagen)
        {
           
            try
            {
                pbxArticulo.Load(imagen);
            }
            catch (Exception)
            {
                pbxArticulo.Load("https://edteam-media.s3.amazonaws.com/blogs/big/2ab53939-9b50-47dd-b56e-38d4ba3cc0f0.png");
                
            }
        }

        private void dgvArticulo_SelectionChanged(object sender, EventArgs e)
        {
            Articulo articulo = (Articulo)dgvArticulo.CurrentRow.DataBoundItem;
            cargarImagen(articulo.ImagenUrl);
        }
    }
}
