   M S B A M L   `   `   `  ????	  OMEGA1    +WpfApplication1.EditServicesAndReservations   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .??5      ?    Bg#???   6   .+?5	   
   ? $??12,12,12,57q?6   ??5
      ?? 5        ?? EndPoint$  0.5,1&?6&    ?? 
StartPoint$ 0.5,0&?67   .??5      ?  ? Color$ White??6#    ? Offset$ 0Q?61   ? 5      $ 	#FF6DD9FF??6#   $ 1Q?65   5      .+?5      ?? -   5         ??Name$ CaseInvoiceNo??5   ?    ?? AutoWordSelection . $??28??6/   $??Center=?6;    ?? 
IsReadOnly . ??.  $??307,32,259,0q?6|    ?? 	MaxLength$
 1000??6?   $??16??6?   $	??Top=?6?   $??70??6?   $y?Hidden=?6?   .??6?   ??5      ?? 5      $  0.5,1&?6*   $ 0.5,0&?6;   .??5      ? $ White??6'   $ 0Q?65   ? 5      $ 	#37FFFF00??6'   $ 1Q?69   5      .??5   &   5      ?? 5      -   5        $??txtID??5   Q   $??28??6   $
??Left=?6"   $??
417,14,0,0q?6=   $	??Top=?6^   $	??120??6v   $ 10??6?   $??0??6?    . .??6?   ??5      ?? 5      $  0.5,1&?6*   $ 0.5,0&?6;   .??5      ? $ White??6'   $ 0Q?65   ? 5      $ 	#37FFFF00??6'   $ 1Q?69   5      .??5   &   5      ?? 5       -   5        $??
lblDayDate??5    ?   ???	#FF078200$??1q?6,   $??التاريخ??6@   ???	#FF078200$??28??6i   $
??Left=?6u   $??Center=?6?   $??
291,44,0,0q?6?    ? UseLayoutRounding . $	??Top=?6?   $	??120??6  (  "System.Windows.Controls.DatePicker  5!      -   5        $??DayDate??5!   n   	   FirstDayOfWeek$	 Saturday=?6   $??28??63   $
??Left=?6?   $??
417,44,0,0q?6Z   $??1??6}   $	??Top=?6?   $	??129??6?   ?? 5"      -   5        $??lblNotes??5"   ?   ???	#FF078200$??1q?6,   $??ملاحظات??6@   ???	#FF078200$??28??6i   $
??Left=?6u   $??Center=?6?   $??289,437,0,11q?6?    . $??Bottom=?6?   $	??120??6  ?? 5#      -   5        $??Notes??5#   b    . $??28??6/   $??417,437,0,11q?6;   $
 1000??6Q   $??12??6o   $??Bottom=?6}   $	??385??6?   $
??Left=?6?   .??6?   ??5$      ?? 5%      $  0.5,1&?6*   $ 0.5,0&?6;   .??5&      ? $ White??6'   $ 0Q?65   ? 5'      $ 	#37FFFF00??6'   $ 1Q?69   5(      .??5)   &   5*      ?? 5+      -   5        $??lblId??5+   ?   ???	#FF078200$??1q?6,   $!??مسلسل الفاتورة??6@   ???	#FF078200$??28??6p   $
??Left=?6|   $??Center=?6?   $??
291,14,0,0q?6?    . $	??Top=?6?   $	??120??6  ?? 5,      -   5        $ CaseName??5,   ?    . $??28??6/   $
??Left=?6;    . ??.  $??
540,78,0,0q?6z   $	 100??6?   $??1??6?   $	??Top=?6?   $	??284??6?   .??6?   ??5-      ?? 5.      $  0.5,1&?6*   $ 0.5,0&?6;   .??5/      ? $ White??6'   $ 0Q?65   ? 50      $ 	#37FFFF00??6'   $ 1Q?69   51      .??52   &   53      ?? 54      -	   5        
 ??Name$
 	lblCaseId??54   ?   ???	#FF078200$??1q?6,   $??Patient??6@   ???	#FF078200$??28??6i   $
??Left=?6u   $??Center=?6?   $??
291,77,0,0q?6?    . $	??Top=?6?   $	??120??6  ?? 55      -
   5        $ CaseId??55   y    . $??28??6/   $
??Left=?6;   $??
417,78,0,0q?6V   $ 10??6j   $??5??6?   $	??Top=?6?   $	??118??6?   .??6?   ??56      ?? 57      $  0.5,1&?6*   $ 0.5,0&?6;   .??58      ? $ White??6'   $ 0Q?65   ? 59      $ 	#37FFFF00??6'   $ 1Q?69   5:      .??5;   &   5<      ?? 5=      -   5         ??Name$ 	CompanyId??5=   {   $??28??6   $
??Left=?6#   $??
832,77,0,0q?6g   $??2??6?   $	??Top=?6?   $	??187??6?   $ ?? IsSynchronizedWithCurrentItem 6>   Z? ??5>      ?? 5?      $  0.5,1&?6*   $ 0.5,0&?6;   .??5@      ? $ White??6'   $ 0Q?65   ? 5A      $ 	#37FFFF00??6'   $ 1Q?69   5B      5D      7  1System.Windows.Forms.Integration.WindowsFormsHost  5E      -   5          Name$	 WFH??5E   6   $??292,111,10,44q?6      TabIndex$ 10??6Y      
Background 6C   Z? 6Y   ?? 5F      -   5        $ CurrentShift??5F   y    . $??28??6/   $??Right=?6;   $??
0,0,128,11q?6W   $ 5??6k   $??2??6?   $??Bottom=?6?   $	??118??6?   {?.  .??6?   ??5G      ?? 5H      $  0.5,1&?6*   $ 0.5,0&?6;   .??5I      ? $ White??6'   $ 0Q?65   ? 5J      $ 	#37FFFF00??6'   $ 1Q?69   5K      .??5L   &   5M      ?? 5N      -   5        $ InsertedDate??5N   x    . $??28??6/   $??Right=?6;   $??	0,0,10,11q?6W   $ 5??6j   $??2??6?   $??Bottom=?6?   $	??118??6?   {?.  .??6?   ??5O      ?? 5P      $  0.5,1&?6*   $ 0.5,0&?6;   .??5Q      ? $ White??6'   $ 0Q?65   ? 5R      $ 	#37FFFF00??6'   $ 1Q?69   5S      .??5T   &   5U      ?? 5V      -   5        $ txtFlag??5V   P   $??28??6   $
??Left=?6"   $??	297,0,0,0q?6=   $	??Top=?6a   $??62??6y   $ 10??6?   $??0??6?    . $y?Hidden=?6?   .??6?   ??5W      ?? 5X      $  0.5,1&?6*   $ 0.5,0&?6;   .??5Y      ? $ White??6'   $ 0Q?65   ? 5Z      $ 	#37FFFF00??6'   $ 1Q?69   5[      .??5\   &   5]      5^      ?? 5_   
   -   5        $??btnSave??5_   9   $??
طباعة??6   $??390,0,552,10q?6#   $??13??6H   $??Bottom=?6V   $??Center=?6q   $	??150??6?    
 BgSave#???  6?   $??42??6?   ?? 5`   
   -   5        $??btnNew??5`   U   $??جديد??6   $??Center=?6!   $??624,0,397,10q?6?   $??14??6c   $??Bottom=?6q   $??71??6?    	 BgNew#???  6?   $??42??6?   ?? 5a   
   -   5        $??	btnDelete??5a   7   $??مسح??6   $??714,0,306,54q?6!   $??15??6H   $??Bottom=?6V   $??Center=?6q   $??72??6?     BgDelete#???  6?   $y?Hidden=?6?   ?? 5b   
   -   5         ??Name$ btnSaveWithoutPrint??5b   7   $??حفظ??6   $??545,0,474,10q?6!   $??13??6T   $??Bottom=?6b   $??Center=?6}   $??73??6?   #???  6?   ??.  $??42??6?   5c      5d      