   M S B A M L   `   `   `  ����M  ISaveLocalCloudSync, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:SaveLocalCloudSync.WindowsSaveLocalCloudSync.Windows  5   	   .    (SaveLocalCloudSync.Windows.NewGameWindow   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   :local(clr-namespace:SaveLocalCloudSync.Windows     5   	   _ [MaterialDesignThemes.Wpf, Version=5.1.0.0, Culture=neutral, PublicKeyToken=df2a72020bd7962aFmaterialDesign1http://materialdesigninxaml.net/winfx/xaml/themes  5	      :md1http://materialdesigninxaml.net/winfx/xaml/themes  5   	     � Title$  NewGameWindow��5   	   $	��232��6   $	��823��6,    � 
ResizeMode$ NoResize=�68    � WindowStartupLocation$ CenterScreen=�6N      MaterialDesignBody#���   5
      $��Regular"�5      $��13(�5      &   System.Windows.Media.TextOptions   TextFormattingMode$ Ideal=�5         TextRenderingMode$
 Auto=�5        MaterialDesignPaper#���  5        MaterialDesignFont#���  5      .��5      � .+�5   
   �� $
��Name��6   $
��Left=�6   $��	22,15,0,0q�6:   $	��Top=�6M   �� 5   
   $��
Local data��6   $
��Left=�6%   $��	10,45,0,0q�6@   $	��Top=�6S   �� 5   
   $��
Cloud data��6   $
��Left=�6%   $��	10,75,0,0q�6@   $	��Top=�6S   �� 5   
   -   5         ��Name$ textboxName��5      $
��Left=�6'   $��	82,14,0,0q�6B    �� TextWrapping$
 Wrap=�6U   $�� ��6i   $	��Top=�6q   $	��708��6�   �� 5   
   -   5        $ textboxLocalData��5      $
��Left=�6,   $��	82,44,0,0q�6G   $
 Wrap=�6Z   $�� ��6n   $	��Top=�6v   $	��708��6�   �� 5   
   -   5        $ textboxCloudData��5      $
��Left=�6,   $��	82,74,0,0q�6G   $
 Wrap=�6Z   $�� ��6n   $	��Top=�6v   $	��708��6�   �� 5   
   -   5         ��Name$ 
buttonSave��5        MaterialDesignFlatButton#��[ 6'   
���"�"�$
��Left=�6s   $��
12,118,0,0q�6�   $	��234��6�   $��67��6�   $	��Top=�6�   .��5      ��  �� Orientation$ 
Horizontal=�6   .+�5      '  !MaterialDesignThemes.Wpf.PackIcon  	   Kind$	 Approve=�6   $��16��6-   $��16��68   $��Center=�6D   $
��Left=�6_   $��5,0,5,0q�6z   5      5      �� 5   
   -   5        $ buttonCancel��5      #��[ 6(   
���\\��$
��Left=�6r   $��251,118,0,0q�6�   $	��234��6�   $��67��6�   $	��Top=�6�   .��5      �� $ 
Horizontal=�6   .+�5         $	 Cancel=�6   $��16��6,   $��16��67   $��Center=�6C   $
��Left=�6^   $��5,0,5,0q�6y   5!      5"      5$      5%      