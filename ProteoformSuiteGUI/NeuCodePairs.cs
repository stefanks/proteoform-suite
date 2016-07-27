﻿using ProteoformSuiteInternal;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;


namespace ProteoformSuite
{
    public partial class NeuCodePairs : Form
    {
        bool formLoadEvent = true; 

        public NeuCodePairs()
        {
            InitializeComponent();
            this.ct_IntensityRatio.MouseMove += new MouseEventHandler(ct_IntensityRatio_MouseMove);
            this.ct_LysineCount.MouseMove += new MouseEventHandler(ct_LysineCount_MouseMove);
        }

        public void NeuCodePairs_Load(object sender, EventArgs e)
        {
            InitializeParameterSet();
            GraphNeuCodePairs();
            FillNeuCodePairsDGV();
            formLoadEvent = false;
        }

        public void FillNeuCodePairsDGV()
        {
            DisplayUtility.FillDataGridView(dgv_RawExpNeuCodePairs, Lollipop.raw_neucode_pairs);
            FormatNeuCodePairsDGV();
        }

        public void GraphNeuCodePairs()
        {
            GraphLysineCount();
            GraphIntensityRatio();
        }

        private void InitializeParameterSet()
        {
            IRatMaxAcceptable.Minimum = 0; IRatMaxAcceptable.Maximum = 20; IRatMaxAcceptable.Value = Lollipop.max_intensity_ratio;
            IRatMinAcceptable.Minimum = 0; IRatMinAcceptable.Maximum = 20; IRatMinAcceptable.Value = Lollipop.min_intensity_ratio;

            KMaxAcceptable.Minimum = 0; KMaxAcceptable.Maximum = 28; KMaxAcceptable.Value = Lollipop.max_lysine_ct;
            KMinAcceptable.Minimum = 0; KMinAcceptable.Maximum = 28; KMinAcceptable.Value = Lollipop.min_lysine_ct;


        }

        private void FormatNeuCodePairsDGV()
        {
            //round table values
            dgv_RawExpNeuCodePairs.Columns["monoisotopic_mass"].DefaultCellStyle.Format = "0.####";
            dgv_RawExpNeuCodePairs.Columns["delta_mass"].DefaultCellStyle.Format = "0.####";
            dgv_RawExpNeuCodePairs.Columns["weighted_monoisotopic_mass"].DefaultCellStyle.Format = "0.####";
            dgv_RawExpNeuCodePairs.Columns["corrected_mass"].DefaultCellStyle.Format = "0.####";
            dgv_RawExpNeuCodePairs.Columns["rt_apex"].DefaultCellStyle.Format = "0.##";
            dgv_RawExpNeuCodePairs.Columns["relative_abundance"].DefaultCellStyle.Format = "0.####";
            dgv_RawExpNeuCodePairs.Columns["fract_abundance"].DefaultCellStyle.Format = "0.####";
            dgv_RawExpNeuCodePairs.Columns["intensity_sum"].DefaultCellStyle.Format = "0.####";
            dgv_RawExpNeuCodePairs.Columns["intensity_ratio"].DefaultCellStyle.Format = "0.####";

            //set column header
            dgv_RawExpNeuCodePairs.Columns["monoisotopic_mass"].HeaderText = "Light Monoisotopic Mass";
            dgv_RawExpNeuCodePairs.Columns["id_light"].HeaderText = "Neucode Light ID";
            dgv_RawExpNeuCodePairs.Columns["id_heavy"].HeaderText = "Neucode Heavy ID";
            dgv_RawExpNeuCodePairs.Columns["delta_mass"].HeaderText = "Light Delta Mass";
            dgv_RawExpNeuCodePairs.Columns["weighted_monoisotopic_mass"].HeaderText = "Light Weighted Monoisotopic Mass";
            dgv_RawExpNeuCodePairs.Columns["corrected_mass"].HeaderText = "Light Corrected Mass";
            dgv_RawExpNeuCodePairs.Columns["rt_apex"].HeaderText = "Light Apex RT";
            dgv_RawExpNeuCodePairs.Columns["relative_abundance"].HeaderText = "Light Relative Abundance";
            dgv_RawExpNeuCodePairs.Columns["fract_abundance"].HeaderText = "Light Fractional Abundance";
            dgv_RawExpNeuCodePairs.Columns["intensity_sum_olcs"].HeaderText = "Light Intensity Sum for Overlapping Charge States";
            dgv_RawExpNeuCodePairs.Columns["file_origin"].HeaderText = "Filename";
            dgv_RawExpNeuCodePairs.Columns["scan_range"].HeaderText = "Scan Range";
            dgv_RawExpNeuCodePairs.Columns["rt_range"].HeaderText = "RT Range";
            dgv_RawExpNeuCodePairs.Columns["num_charge_states"].HeaderText = "No. Charge States";
            dgv_RawExpNeuCodePairs.Columns["intensity_ratio"].HeaderText = "Intensity Ratio";
            dgv_RawExpNeuCodePairs.Columns["lysine_count"].HeaderText = "Lysine Count";

            dgv_RawExpNeuCodePairs.Columns["id"].Visible = false;
            dgv_RawExpNeuCodePairs.Columns["intensity_sum"].Visible = false;
            dgv_RawExpNeuCodePairs.AllowUserToAddRows = false;
        }

        private void GraphIntensityRatio()
        {
            DataTable intensityRatioHistogram = new DataTable();
            intensityRatioHistogram.Columns.Add("intRatio", typeof(double));
            intensityRatioHistogram.Columns.Add("numPairsAtThisIntRatio", typeof(int));

            int ymax = 0;
            for (double i = 0; i <= 20; i = i + 0.05)
            {
                List<NeuCodePair> proteoforms_by_intensityRatio = Lollipop.raw_neucode_pairs.Where(p => p.intensity_ratio >= i - 0.025 && p.intensity_ratio < i + 0.025).ToList();
                if (proteoforms_by_intensityRatio.Count > ymax)
                    ymax = proteoforms_by_intensityRatio.Count;
                intensityRatioHistogram.Rows.Add(i, proteoforms_by_intensityRatio.Count);
            }
            ct_IntensityRatio.DataSource = intensityRatioHistogram;
            ct_IntensityRatio.DataBind();

            ct_IntensityRatio.Series["intensityRatio"].XValueMember = "intRatio";
            ct_IntensityRatio.Series["intensityRatio"].YValueMembers = "numPairsAtThisIntRatio";

            yMaxIRat.Minimum = 0; yMaxIRat.Maximum = ymax; yMaxIRat.Value = ymax;
            yMinIRat.Minimum = 0; yMinIRat.Maximum = ymax; yMinIRat.Value = 0;
            xMaxIRat.Minimum = 0; xMaxIRat.Maximum = 20; xMaxIRat.Value = 20;
            xMinIRat.Minimum = 0; xMinIRat.Maximum = 20; xMinIRat.Value = 0;

            ct_IntensityRatio.ChartAreas[0].AxisX.Title = "Intensity Ratio of a Pair";
            ct_IntensityRatio.ChartAreas[0].AxisY.Title = "Number of NeuCode Pairs";
        }

        private void GraphLysineCount()
        {
            DataTable lysCtHistogram = new DataTable();
            lysCtHistogram.Columns.Add("numLysines", typeof(int));
            lysCtHistogram.Columns.Add("numPairsAtThisLysCt", typeof(int));

            int ymax = 0;
            for (int i = 0; i <= 28; i++)
            {
                List<NeuCodePair> pf_by_lysCt = Lollipop.raw_neucode_pairs.Where(p => p.lysine_count == i).ToList();
                if (pf_by_lysCt.Count > ymax)
                    ymax = pf_by_lysCt.Count;
                lysCtHistogram.Rows.Add(i, pf_by_lysCt.Count);
            }
            ct_LysineCount.DataSource = lysCtHistogram;
            ct_LysineCount.DataBind();

            ct_LysineCount.Series["lysineCount"].XValueMember = "numLysines";
            ct_LysineCount.Series["lysineCount"].YValueMembers = "numPairsAtThisLysCt";

            yMaxKCt.Minimum = 0; yMaxKCt.Maximum = ymax; yMaxKCt.Value = ymax;
            yMinKCt.Minimum = 0; yMinKCt.Maximum = ymax; yMinKCt.Value = 0;
            xMaxKCt.Minimum = 0;                         xMaxKCt.Value = 28;
            xMinKCt.Minimum = 0; xMinKCt.Maximum = 28; xMinKCt.Value = 0;

            ct_LysineCount.ChartAreas[0].AxisX.Title = "Lysine Count";
            ct_LysineCount.ChartAreas[0].AxisY.Title = "Number of NeuCode Pairs";            
        }

        Point? ct_intensityRatio_prevPosition = null;
        ToolTip ct_intensityRatio_tt = new ToolTip();

        void ct_IntensityRatio_MouseMove(object sender, MouseEventArgs e)
        {
            DisplayUtility.tooltip_graph_display(ct_intensityRatio_tt, e, ct_IntensityRatio, ct_intensityRatio_prevPosition);
        }

        Point? ct_LysineCount_prevPosition = null;
        ToolTip ct_LysineCount_tt = new ToolTip();

        void ct_LysineCount_MouseMove(object sender, MouseEventArgs e)
        {
            DisplayUtility.tooltip_graph_display(ct_LysineCount_tt, e, ct_LysineCount, ct_LysineCount_prevPosition);
        }

        private void yMaxKCt_ValueChanged(object sender, EventArgs e)
        {
            ct_LysineCount.ChartAreas[0].AxisY.Maximum = double.Parse(yMaxKCt.Value.ToString());
        }

        private void yMinKCt_ValueChanged(object sender, EventArgs e)
        {
            ct_LysineCount.ChartAreas[0].AxisY.Minimum = double.Parse(yMinKCt.Value.ToString());
        }

        private void xMinKCt_ValueChanged(object sender, EventArgs e)
        {
            ct_LysineCount.ChartAreas[0].AxisX.Minimum = double.Parse(xMinKCt.Value.ToString());
        }

        private void xMaxKCt_ValueChanged(object sender, EventArgs e)
        {
            ct_LysineCount.ChartAreas[0].AxisX.Maximum = double.Parse(xMaxKCt.Value.ToString());
        }

        private void yMaxIRat_ValueChanged(object sender, EventArgs e)
        {
            ct_IntensityRatio.ChartAreas[0].AxisY.Maximum = double.Parse(yMaxIRat.Value.ToString());
        }

        private void yMinIRat_ValueChanged(object sender, EventArgs e)
        {
            ct_IntensityRatio.ChartAreas[0].AxisY.Minimum = double.Parse(yMinIRat.Value.ToString());
        }

        private void xMinIRat_ValueChanged(object sender, EventArgs e)
        {
            ct_IntensityRatio.ChartAreas[0].AxisX.Minimum = double.Parse(xMinIRat.Value.ToString());
        }

        private void xMaxIRat_ValueChanged(object sender, EventArgs e)
        {
            ct_IntensityRatio.ChartAreas[0].AxisX.Maximum = double.Parse(xMaxIRat.Value.ToString());
        }

        private void KMinAcceptable_ValueChanged(object sender, EventArgs e)
        {
            if (!formLoadEvent)
            {
                Lollipop.min_lysine_ct = KMinAcceptable.Value;
                //if KMin increased, incorrect values un-accepted
                List<NeuCodePair> selected_pf = Lollipop.raw_neucode_pairs.Where(p => p.lysine_count < Lollipop.min_lysine_ct).ToList();
                Parallel.ForEach(selected_pf, p => { p.accepted = false; });
                //if Kmin is decreased, correct values are re-accepted.  
                List<NeuCodePair> unselected_pf = Lollipop.raw_neucode_pairs.Where(p => p.lysine_count > Lollipop.min_lysine_ct
                    && p.lysine_count < Lollipop.max_lysine_ct).ToList();
                Parallel.ForEach(unselected_pf, p => { p.accepted = true; });
                dgv_RawExpNeuCodePairs.Refresh();
            }
        }

        private void KMaxAcceptable_ValueChanged(object sender, EventArgs e)
        {
            if (!formLoadEvent)
            {
                Lollipop.max_lysine_ct = KMaxAcceptable.Value;
                //if KMax is decreased, incorrect values are un-accepted
                List<NeuCodePair> selected_pf = Lollipop.raw_neucode_pairs.Where(p => p.lysine_count > Lollipop.max_lysine_ct).ToList();
                Parallel.ForEach(selected_pf, p => { p.accepted = false; });
                //if Kmax  is increased, correct values are re-accepted.  
                List<NeuCodePair> unselected_pf = Lollipop.raw_neucode_pairs.Where(p => p.lysine_count < Lollipop.max_lysine_ct
                   && p.lysine_count > Lollipop.min_lysine_ct).ToList();
                Parallel.ForEach(unselected_pf, p => { p.accepted = true; });
                dgv_RawExpNeuCodePairs.Refresh();
            }
        }

        private void IRatMinAcceptable_ValueChanged(object sender, EventArgs e)
        {
            if (!formLoadEvent)
            {
                Lollipop.min_intensity_ratio = IRatMinAcceptable.Value;
                //if IMin is increaed, incorrect values are un-accepted
                List<NeuCodePair> selected_pf = Lollipop.raw_neucode_pairs.Where(p => p.intensity_ratio < Convert.ToDouble(Lollipop.min_intensity_ratio)).ToList();
                Parallel.ForEach(selected_pf, p => { p.accepted = false; });
                //if IMin is decreased, correct values are re-accepted
                List<NeuCodePair> unselected_pf = Lollipop.raw_neucode_pairs.Where(p => p.intensity_ratio > Convert.ToDouble(Lollipop.min_intensity_ratio)
                   && p.intensity_ratio < Convert.ToDouble(Lollipop.max_intensity_ratio)).ToList();
                Parallel.ForEach(unselected_pf, p => { p.accepted = true; });
                dgv_RawExpNeuCodePairs.Refresh();
            }
        }

        private void IRatMaxAcceptable_ValueChanged(object sender, EventArgs e)
        {
            if (!formLoadEvent)
            {
                Lollipop.max_intensity_ratio = IRatMaxAcceptable.Value;
                //if Imax is decreased, incorrect values are un-accepted
                List<NeuCodePair> selected_pf = Lollipop.raw_neucode_pairs.Where(p => p.intensity_ratio > Convert.ToDouble(Lollipop.max_intensity_ratio)).ToList();
                Parallel.ForEach(selected_pf, p => { p.accepted = false; });
                //if Imax is increased, correct values are re-accepted
                List<NeuCodePair> unselected_pf = Lollipop.raw_neucode_pairs.Where(p => p.intensity_ratio < Convert.ToDouble(Lollipop.max_intensity_ratio)
                  && p.intensity_ratio > Convert.ToDouble(Lollipop.min_intensity_ratio)).ToList();
                Parallel.ForEach(unselected_pf, p => { p.accepted = true; });
                dgv_RawExpNeuCodePairs.Refresh();
            }
        }
    }
}