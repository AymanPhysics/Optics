   M S B A M L   `   `   `  ????	  OMEGA&     WpfApplication1.InvestorsProfits   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      .??5      ?    Bg#???   6   .+?5	   
   ? $??12,12,12,104q?6   ??5
      ?? 5        ?? EndPoint$  0.5,1&?6&    ?? 
StartPoint$ 0.5,0&?67   .??5      ?  ? Color$ White??6#    ? Offset$ 0Q?61   ? 5      $ 	#FF6DD9FF??6#   $ 1Q?65   5      .+?5      ?? -   5        $??txtFlag??5       ?? AutoWordSelection . $??28??6/   $??Center=?6;   $??297,24,321,0q?6X    ?? 	MaxLength$
 1000??6n   $??6??6?   $	??Top=?6?   $??36??6?   $y?Hidden=?6?   .??6?   ??5      ?? 5      $  0.5,1&?6*   $ 0.5,0&?6;   .??5      ? $ White??6'   $ 0Q?65   ? 5      $ 	#37FFFF00??6'   $ 1Q?69   5      .??5   &   5      ?? 5      -   5        $??txtID??5   Q   $??28??6   $
??Left=?6"   $??
168,10,0,0q?6=   $	??Top=?6^   $	??120??6v   $ 10??6?   $??1??6?    . .??6?   ??5      ?? 5      $  0.5,1&?6*   $ 0.5,0&?6;   .??5      ? $ White??6'   $ 0Q?65   ? 5      $ 	#37FFFF00??6'   $ 1Q?69   5      .??5    &   5!      ?? 5"      -   5        $??
lblDayDate??5"   ?   ???	#FF078200$??1q?6,   $??التاريخ??6@   ???	#FF078200$??28??6i   $
??Left=?6u   $??Center=?6?   $??	42,44,0,0q?6?    ? UseLayoutRounding . $	??Top=?6?   $	??120??6
  (  "System.Windows.Controls.DatePicker  5#      -   5        $??DayDate??5#   n      FirstDayOfWeek$ Saturday=?6   $??28??63   $
??Left=?6?   $??
168,44,0,0q?6Z   $??2??6}   $	??Top=?6?   $	??129??6?   ?? 5$      -   5         ??Name$ lblID??5$   ?   ???	#FF078200$??1q?6,   $??المسلسل??6@   ???	#FF078200$??28??6i   $
??Left=?6u   $??Center=?6?   $??	42,10,0,0q?6?    . $	??Top=?6?   $	??120??6  ?? 5%      -   5        	 ??Name$	 IsPosted??5%   S   $??IsPosted??6   $??28??6*   $??Center=?66   $??3??6e   $	??Top=?6r   $??298,63,281,0q?6?   ??.  $
??Left=?6?   $??Center=?6?   
 J? RenderTransformOrigin$
 3.646,-0.875&?6?   $??57??6  $y?Hidden=?6'  7  1System.Windows.Forms.Integration.WindowsFormsHost  5&      -   5          Name$	 WFH??5&   5   $??10,145,10,43q?6      TabIndex$ 10??6X      
Background 6B   Z? 6X   ?? 5'      -   5         ??Name$ Total1??5'       . $??28??6/   $??Center=?6;   $??168,0,419,10q?6X   $
 1000??6n   $??17??6?   $??Bottom=?6?   $	??106??6?    ?? 
IsReadOnly . ??.  .??6?   ??5(      ?? 5)      $  0.5,1&?6*   $ 0.5,0&?6;   .??5*      ? $ White??6'   $ 0Q?65   ? 5+      $ 	#37FFFF00??6'   $ 1Q?69   5,      .??5-   &   5.      ?? 5/      -	   5        $ Label6_Copy??5/   ?   ???	#FF078200$??1q?6,   $!??إجمالي الأرصدة??6@   ???	#FF078200$??28??6p   $??Center=?6|   $??Center=?6?   $??42,0,531,10q?6?    . $??Bottom=?6   $	??120??6  ?? 50      -
   5        $	 Perc1??50   W   $ ??نسبة من الرصيد??6   $??16??60   $
??Left=?6<   $??17??6f   $	??Top=?6t   $	??125??6?   $??	42,83,0,0q?6?   ?? 51      -   5        $	 Perc2??51   U   $??نسبة من مبلغ??6   $??16??6.   $
??Left=?6:   $??17??6d   $	??Top=?6r   $	??125??6?   $??
42,116,0,0q?6?   ?? 52      -   5        $
 Val1??52   Q   $??28??6   $
??Left=?6"   $??
168,77,0,0q?6=   $	??Top=?6_   $	??120??6w   $ 10??6?   $??1??6?    . .??6?   ??53      ?? 54      $  0.5,1&?6*   $ 0.5,0&?6;   .??55      ? $ White??6'   $ 0Q?65   ? 56      $ 	#37FFFF00??6'   $ 1Q?69   57      .??58   &   59      ?? 5:      -   5        $
 Val2??5:   R   $??28??6   $
??Left=?6"   $??168,110,0,0q?6=   $	??Top=?6`   $	??120??6x   $ 10??6?   $??1??6?    . .??6?   ??5;      ?? 5<      $  0.5,1&?6*   $ 0.5,0&?6;   .??5=      ? $ White??6'   $ 0Q?65   ? 5>      $ 	#37FFFF00??6'   $ 1Q?69   5?      .??5@   &   5A      ?? 5B      -   5        $ 
lblID_Copy??5B   ?   ???	#FF078200$??1q?6,   $??%??6@   ???	#FF078200$??28??6c   $
??Left=?6o   $??Center=?6?   $??
293,77,0,0q?6?    . $	??Top=?6?   $??33??6  ?? 5C      -   5        $ Total2??5C       . $??28??6/   $??Center=?6;   $??442,0,145,10q?6X   $
 1000??6n   $??17??6?   $??Bottom=?6?   $	??106??6?    . ??.  .??6?   ??5D      ?? 5E      $  0.5,1&?6*   $ 0.5,0&?6;   .??5F      ? $ White??6'   $ 0Q?65   ? 5G      $ 	#37FFFF00??6'   $ 1Q?69   5H      .??5I   &   5J      ?? 5K      -   5        $ Label6_Copy1??5K   ?   ???	#FF078200$??1q?6,   $!??إجمالى الأرباح??6@   ???	#FF078200$??28??6p   $??Center=?6|   $??Center=?6?   $??316,0,257,10q?6?    . $??Bottom=?6  $	??120??6  ?? 5L      -   5         ??Name$ btncalc??5L   <   $??احتساب??6   $??
461,77,0,0q?6(   $??20??6M   $	??Top=?6[   $
??Left=?6s   $	??199??6?    
 BgSave#???  6?   ??.  $??68??6?   ?? 5M      -   5        $	 IsRound??5M   ]   $*??$التقريب لأقرب 5 جنيه??6   $??16??66   $
??Left=?6B   $??17??6n   $	??Top=?6|   $	??125??6?   $??
331,83,0,0q?6?   ?? 5N      -   5        $ lblDayDate_Copy??5N   ?   ???	#FF078200$??1q?6,   $#??خصم أقساط عن شهر??6@   ???	#FF078200$??28??6r   $
??Left=?6~   $??Center=?6?   $??
336,42,0,0q?6?    . $	??Top=?6  $	??120??6    5O      -   5          Name$ DayDate2??5O   n   $ Saturday=?6   $??28??63   $
??Left=?6?   $??
462,42,0,0q?6Z   $??2??6?   $	??Top=?6?   $??90??6?    ? 	Resources 5P      :  4System.Windows.Controls.Primitives.DatePickerTextBox ?? 
TargetType ?? Property ?? Value ?? Path ?? StringFormat ?? RelativeSource ?? AncestorType??%?   '        ?? 5Q      "  6   .?5R      ??  ? ?? 5S      ?? 5T   "   .R?5U   &   ?? $ PART_TextBox??6.   ??5V      ?? $ SelectedDate?$ {0:MM-yyyy}?? ?? "  5X   #   5Z      5[      5\      5]      5^      ?? 5_   
   -   5        $??btnFirst??5_   T   $??أول??6   $??Center=?6    $??186,0,420,12q?6>   $??13??6d   $??Bottom=?6r   $??72??6?   #???  6?   ??.  6?   ?? 5`   
   -   5        $??
btnPrevios??5`   U   $??سابق??6   $??Center=?6!   $??264,0,342,12q?6?   $??14??6g   $??Bottom=?6u   $??72??6?   #???  6?   ??.  6?   ?? 5a   
   -   5        $??btnNext??5a   U   $??تالى??6   $??Center=?6!   $??342,0,264,12q?6?   $??15??6d   $??Bottom=?6r   $??72??6?   #???  6?   ??.  6?   ?? 5b   
   -   5        $??btnLast??5b   U   $??أخير??6   $??Center=?6!   $??420,0,186,12q?6?   $??16??6d   $??Bottom=?6r   $??72??6?   #???  6?   ??.  6?   ?? 5c   
   -   5        $??btnSave??5c   7   $??حفظ??6   $??186,0,342,58q?6!   $??20??6F   $??Bottom=?6T   $??Center=?6o   $	??150??6?   #???  6?   ??.  6?   ?? 5d   
   -   5        $??btnNew??5d   U   $??جديد??6   $??Center=?6!   $??342,0,264,58q?6?   $??12??6c   $??Bottom=?6q   $??72??6?    	 BgNew#???  6?   ??.  6?   ?? 5e   
   -   5        $??	btnDelete??5e   7   $??مسح??6   $??420,0,186,58q?6!   $??12??6H   $??Bottom=?6V   $??Center=?6q   $??72??6?     BgDelete#???  6?   ??.  6?   5f      5g      