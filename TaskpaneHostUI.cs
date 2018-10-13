using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SolidWorks.Interop.sldworks;
using System.Diagnostics;
using SolidWorks.Interop.swconst;

namespace MpcExport
{
    public partial class TaskpaneHostUI : UserControl
    {
        int errors = 0;
        int warnings = 0;
        SldWorks swApp;
        ModelDoc2 swModelDoc = default(ModelDoc2);
        ModelDocExtension swModelDocExt = default(ModelDocExtension);
        AssemblyDoc swAssDoc = default(AssemblyDoc);
        Feature swFeat = default(Feature);
        FeatureManager swFeatMgr = default(FeatureManager);
        CommentFolder swCommentFolder = default(CommentFolder);
        Comment swComment = default(Comment);
        int nbrComments = 0;
        string sFeatType = null;
        object[] vComments = null;
        long i = 0;

        public TaskpaneHostUI()
        {
            InitializeComponent();
        }

        public void SetSwApp(SldWorks swApp)
        {
            this.swApp = swApp;
        }

        private void ExportButtonClick(object sender, EventArgs e)
        {
            swModelDoc = swApp.ActiveDoc;
            swModelDocExt = swModelDoc.Extension;
            swAssDoc = swApp.ActiveDoc as AssemblyDoc;
            swFeat = (Feature)swModelDoc.FirstFeature();
            swFeatMgr = (FeatureManager)swModelDoc.FeatureManager;
            while ((swFeat != null))
            {
                sFeatType = swFeat.GetTypeName();

                if (sFeatType == "CommentsFolder")
                {
                    swCommentFolder = (CommentFolder)swFeat.GetSpecificFeature2();

                    nbrComments = swCommentFolder.GetCommentCount();
                    vComments = (object[])swCommentFolder.GetComments();
                    for (i = 0; i <= (nbrComments - 1); i++)
                    {
                        swComment = (Comment)vComments[i];
                        string name = swComment.Name;                        
                        Component2 comp = swAssDoc.GetComponentByName(name);
                        swModelDocExt.SelectByID2(comp.GetSelectByIDString(), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAssDoc.HideComponent();
                    }

                    for (i = 0; i <= (nbrComments - 1); i++)
                    {
                        swComment = (Comment)vComments[i];
                        string name = swComment.Name;
                        Component2 comp = swAssDoc.GetComponentByName(name);
                        swModelDocExt.SelectByID2(comp.GetSelectByIDString(), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAssDoc.ShowComponent();
                        swModelDocExt.SetUserPreferenceString((int)swUserPreferenceStringValue_e.swFileSaveAsCoordinateSystem, (int)swUserPreferenceOption_e.swDetailingNoOptionSpecified, "CoordinateSystem");
                        swModelDocExt.SaveAs("C:/Users/Izmar/Documents/vmpc_models/jan2018/stlexport/" + swComment.Text + ".STL", (int)swSaveAsVersion_e.swSaveAsCurrentVersion, (int)swSaveAsOptions_e.swSaveAsOptions_Silent, 0, ref errors, ref warnings);
                        swAssDoc.HideComponent();
                    }
                    for (i = 0; i <= (nbrComments - 1); i++)
                    {
                        swComment = (Comment)vComments[i];
                        string name = swComment.Name;
                        Component2 comp = swAssDoc.GetComponentByName(name);
                        swModelDocExt.SelectByID2(comp.GetSelectByIDString(), "COMPONENT", 0, 0, 0, false, 0, null, 0);
                        swAssDoc.ShowComponent();
                    }
                }

                // Get next feature in FeatureManager design tree
                swFeat = (Feature)swFeat.GetNextFeature();
            }

        }
    }
}
