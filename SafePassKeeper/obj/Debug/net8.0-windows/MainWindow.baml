   M S B A M L   `   `   `  ����I  ESafePassKeeper, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:SafePassKeeperSafePassKeeper  5   	       SafePassKeeper.MainWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:SafePassKeeper     5   	     � Title$  SafePassKeeper��5   	   $	��450��6    $	��800��6-   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $	��13* �6   �� 5      $
��149* �6   �� 5      $
��142* �6   �� 5      $	��96* �6   .+�5   
   �� -   5         ��Name$ AccountListBox��5   +   $y�	Collapsed=�6   $��1��6D   $��0,0,0,60q�6�   �� 5   
   -   5         ��Name$ AddAccountButton��5      $y�	Collapsed=�6,   $��Add Account��6C   $��1��6Z   $��Right=�6k   $��	0,0,10,25q�6�   $��20��6�   $��Bottom=�6�   $��73��6�   �� 5   
   -   5        $ EditAccountButton��5      $y�	Collapsed=�6,   $��Edit Account��6C   $��1��6[   $��110,0,88,25q�6k   $��20��6�   $��Bottom=�6�   �� 5   
   -   5        $ DeleteAccountButton��5      $y�	Collapsed=�61   $��Delete Account��6H   $��1��6a   $��0,0,0,25q�6q   $��20��6�   $��Bottom=�6�   $
��Left=�6�   $��87��6�   �� 5   
   -   5         ��Name$ DetailsTextBox��5   *   $y�	Collapsed=�6   $��2��6B   $��	10,0,10,0q�6R    �� TextWrapping$
 Wrap=�6e   $��TextBox��6y   $	��Top=�6�   $	��374��6�   �� 5   
   -   5        $ SpecifyFilePathButton��5   6   $ ��Set File Path and Password��6   $��3��6V   $��
15,16,24,0q�6f   $��29��6�   $	��Top=�6�   �� 5   
   -   5        $ ExportBackupButton��5      $y�	Collapsed=�6-   $��Export Backup��6D   $��3��6\   $	��100��6l   $��
46,56,46,0q�6x   $��25��6�   $	��Top=�6�   5      5      