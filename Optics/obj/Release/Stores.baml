   M S B A M L   `   `   `  ???? 	  OMEGA    WpfApplication1.Stores   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     @d2http://schemas.microsoft.com/expression/blend/2008     .???    Bg#???   .+?? -   $??GridBackground??$??12,12,12,104q????? -   Img??  ?? ImageSource$  Images/surgey.jpg??.+??? -   $??txtID??$??28??$??
370,28,0,0q?$	??Top=?$	??120?? ?? 	MaxLength$ 10??$??0?? ?? AutoWordSelection . $??Times New Roman)?.??????  ?? EndPoint$ 0.5,1&? ?? 
StartPoint$ 0.5,0&?.???  ? Color$ White?? ? Offset$ 0Q?? $ 	#37FFFF00??$ 1Q?.???? -   $??txtName??$??28??$??
370,58,0,0q?$	??Top=?$	??306??$
 1000??$??1?? . .?????? $ 0.5,1&?$ 0.5,0&?.??? $ White??$ 0Q?? $ 	#37FFFF00??$ 1Q?.???? -   $??lblId??$??Id??$??28??$
??Left=?$??Center=?$??
247,28,0,0q? ? UseLayoutRounding . $	??120??$	??Top=?$??1q????	#FF078200???	#FF078200?? -   $??lblName??$
??Name??$??28??$
??Left=?$??Center=?$??
247,58,0,0q? . $	??120??$	??Top=?$??1q????	#FF078200???	#FF078200?? -   $??	CheckBox1??$??مخزن وهمى??$??16??$
??Left=?$??
370,90,0,0q?$	??Top=?$	??120??$??2???? -   $??	CheckBox2??$<??6استلام التحويلات أوتوماتيكيا??$??16??$
??Left=?$??370,116,0,0q?$	??Top=?$	??150??$??2???? -	   $??Image1??$	??119??$??Center=?$??455,19,99,53q?$*??$/OMEGA;component/Images/no.photo.gif??$
??Fill=?$??Center=?$	??100??$y?Hidden=??? -
   $??btnSetImage??$??Change??$??Center=?$??35??$??Center=?$??82??$y?Hidden=?$??559,40,13,112q????? $ 0.5,1&?$ 0.5,0&?.??? $ 	#B4FF0000??$ 0Q?? $ Black??$ 1Q??? -   $??btnSetNoImage??$??Cancel??$??Center=?$??35??$??Center=?$??82??$y?Hidden=?$??559,81,13,55q????? $ 0.5,1&?$ 0.5,0&?.??? $ 	#B4FF0000??$ 0Q?? $ Black??$ 1Q??? -    ??Name$ lblCurrentDate?????	#FF078200$??1q?$??تاريخ الوردية?????	#FF078200$??28??$
??Left=?$??Center=?$??247,164,0,0q? . $	??Top=?$	??120??(  "System.Windows.Controls.DatePicker  -   	  Name$	 CurrentDate??
   FirstDayOfWeek$
 Saturday=?$??28??$??371,164,0,0q?$??2??$	??128??$	??Top=??? -    ??Name$ CurrentShift??$??28??$??603,164,0,0q?$??5??$	??128?? ?? 
IsEditable .  $	??Top=?$ ?? IsSynchronizedWithCurrentItem Z? ???? $ 0.5,1&?$ 0.5,0&?.??? $ White??$ 0Q?? $ 	#37FFFF00??$ 1Q??? -   $ lblShift?????	#FF078200$??1q?$??الوردية?????	#FF078200$??28??$
??Left=?$??Center=?$??527,164,0,0q? . $	??Top=?$??72???? -   $ StoreUnitId??$??28??$??
603,90,0,0q?$??5??$	??Top=?$	??128?? .   Z? ???? $ 0.5,1&?$ 0.5,0&?.??? $ White??$ 0Q?? $ 	#37FFFF00??$ 1Q??? -   $ lblStoreUnitId?????	#FF078200$??1q?$??وحدة الأصناف?????	#FF078200$??28??$
??Left=?$??Center=?$??
527,90,0,0q? . $	??Top=?$??72???? -    ??Name$ CostCenterName?? . $??28?? ?? 
IsReadOnly . ??.  $??453,200,0,0q?$	 100??$	??888??$	??Top=?$	??222??.?????? $ 0.5,1&?$ 0.5,0&?.??? $ White??$ 0Q?? $ 	#37FFFF00??$ 1Q?.???? -   $ lblCostCenterId?????	#FF078200$??1q?$??مركز التكلفة?????	#FF078200$??28??$
??Left=?$??Center=?$??247,200,0,0q? . $	??Top=?$	??120???? -   $ CostCenterId?? . $??28??$??373,200,0,0q?$ 10??$??6??$	??Top=?$??76??.?????? $ 0.5,1&?$ 0.5,0&?.??? $ White??$ 0Q?? $ 	#37FFFF00??$ 1Q?.???? -   $ lblHomePhone_Copy28?????	#FF078200$??1q?$??WhatsApp?????	#FF078200$??28??$??Center=?$??247,233,0,0q? . $	??120??$
??Left=?$	??Top=??? -   $ WhatsAppNotes??$	??118??$??373,233,0,0q?$
 1000??$??13??$	??Top=? .  ?? AcceptsReturn .  ?? TextWrapping$
 Wrap=?" ?? VerticalScrollBarVisibility$ Visible=?$	??302??.?????? $ 0.5,1&?$ 0.5,0&?.??? $ White??$ 0Q?? $ 	#37FFFF00??$ 1Q?.???? -   $??btnFirst??$??First??$??Center=?$??174,0,422,10q?$??6??$??Bottom=?$??82?? 
 BgSave#???  ??.  ?? -   $??
btnPrevios??$??Previous??$??Center=?$??262,0,334,10q?$??7??$??Bottom=?$??82??#???  ??.  ?? -   $??btnNext??$
??Next??$??Center=?$??350,0,246,10q?$??8??$??Bottom=?$??82??#???  ??.  ?? -   $??btnLast??$
??Last??$??Center=?$??438,0,158,10q?$??9??$??Bottom=?$??82??#???  ??.  ?? -   $??btnSave??$
??Save??$??174,0,334,56q?$??3??$??Bottom=?$??Center=?$	??170??#???  ??.  ?? -   $??btnNew??$	??New??$??Center=?$??350,0,246,56q?$??4??$??Bottom=?$??82?? 	 BgNew#???  ??.  ?? -   $??	btnDelete??$??Delete??$??438,0,158,56q?$??5??$??Bottom=?$??Center=?$??82??  BgDelete#???  ??.  