# How to programmatically reorder Xamarin.Forms ListView (SfListView)

You can programmatically reorder the listview items based on **CheckBox** state in Xamarin.Forms [SfListView](https://help.syncfusion.com/xamarin/listview/overview).

You can also refer the following article.

https://www.syncfusion.com/kb/11933/how-to-programmatically-reorder-xamarin-forms-listview-sflistview

**XAML**

Image loaded in the [SfListView.ItemTemplate](https://help.syncfusion.com/cr/cref_files/xamarin/Syncfusion.SfListView.XForms~Syncfusion.ListView.XForms.SfListView~ItemTemplate.html) and [GestureRecognizers](https://docs.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/gestures/tap) binding with ViewModel [**Command**](https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.tapgesturerecognizer.command?view=xamarin-forms#Xamarin_Forms_TapGestureRecognizer_Command) for [TapGestureRecognizer](https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.tapgesturerecognizer). Also, binding the [CommandParameter](https://docs.microsoft.com/en-us/dotnet/api/xamarin.forms.tapgesturerecognizer.commandparameter?view=xamarin-forms#Xamarin_Forms_TapGestureRecognizer_CommandParameter) as **BindingContext** to get the checked item which handles the **CheckBox** state.

``` xml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms"
             xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"
             xmlns:local="clr-namespace:ListViewXamarin"
             xmlns:syncfusion="clr-namespace:Syncfusion.ListView.XForms;assembly=Syncfusion.SfListView.XForms"
             xmlns:data="clr-namespace:Syncfusion.DataSource;assembly=Syncfusion.DataSource.Portable"
             x:Class="ListViewXamarin.MainPage">
    <ContentPage.BindingContext>
        <local:ViewModel />
    </ContentPage.BindingContext>

    <ContentPage.Resources>
        <ResourceDictionary>
            <local:ImageConverter x:Key="ImageConverter"/>
        </ResourceDictionary>
    </ContentPage.Resources>
    
    <Grid RowSpacing="0">
        <Grid.RowDefinitions>
            <RowDefinition Height="40" />
            <RowDefinition Height="*" />
        </Grid.RowDefinitions>

        <Grid BackgroundColor="#2196F3">
            <Label Text="To Do Items" x:Name="headerLabel" TextColor="White" FontAttributes="Bold" VerticalOptions="Center" HorizontalOptions="Center" />
        </Grid>

        <syncfusion:SfListView x:Name="listView" Grid.Row="1" DragStartMode="OnHold" ItemSize="60" BackgroundColor="#FFE8E8EC" GroupHeaderSize="50" ItemsSource="{Binding ToDoList}" SelectionMode="None">
            <syncfusion:SfListView.DataSource>
                <data:DataSource>
                    <data:DataSource.GroupDescriptors>
                        <data:GroupDescriptor PropertyName="CategoryName">
                            <data:GroupDescriptor.Comparer>
                                <local:CustomGroupComparer/>
                            </data:GroupDescriptor.Comparer>
                        </data:GroupDescriptor>
                    </data:DataSource.GroupDescriptors>
                </data:DataSource>
            </syncfusion:SfListView.DataSource>
           
            <syncfusion:SfListView.GroupHeaderTemplate>
                <DataTemplate>
                    <Grid>
                        <Grid.BackgroundColor>
                            <OnPlatform x:TypeArguments="Color" Android="#eeeeee" iOS="#f7f7f7"/>
                        </Grid.BackgroundColor>
                        <Label Text="{Binding Key}" FontSize="14" TextColor="#333333" FontAttributes="Bold" VerticalOptions="Center" HorizontalOptions="Start" Margin="15,0,0,0" />
                    </Grid>
                </DataTemplate>
            </syncfusion:SfListView.GroupHeaderTemplate>

            <syncfusion:SfListView.ItemTemplate>
                <DataTemplate>
                    <Frame HasShadow="True" BackgroundColor="White" Padding="0">
                        <Frame.InputTransparent>
                            <OnPlatform x:TypeArguments="x:Boolean" Android="True" iOS="False"/>
                        </Frame.InputTransparent>
                        <Grid>
                            <Grid.ColumnDefinitions>
                                <ColumnDefinition Width="55" />
                                <ColumnDefinition Width="*" />
                            </Grid.ColumnDefinitions>
                            <Grid Padding="15,15,15,15">
                                <Image Source="{Binding IsChecked, Converter={StaticResource ImageConverter}}" HeightRequest="50" WidthRequest="50">
                                    <Image.GestureRecognizers>
                                        <TapGestureRecognizer Command="{Binding Path=BindingContext.MarkDoneCommand, Source={x:Reference listView}}" CommandParameter="{Binding .}"/>
                                    </Image.GestureRecognizers>
                                </Image>
                            </Grid>
                            <BoxView Grid.Column="1" Margin="5,3,0,0" BackgroundColor="#333333" HeightRequest="1" WidthRequest="{Binding Source={x:Reference textLabel}, Path=Width}"
                                     VerticalOptions="Center" HorizontalOptions="Start" IsVisible="{Binding IsChecked}" />
                            <Label x:Name="textLabel" Grid.Column="1" Text="{Binding Name}" FontSize="15" TextColor="#333333" VerticalOptions="Center" HorizontalOptions="Start" Margin="5,0,0,0" />
                        </Grid>
                    </Frame>
                </DataTemplate>
            </syncfusion:SfListView.ItemTemplate>
        </syncfusion:SfListView>
    </Grid>
</ContentPage>
```

**C#**

**MarkDoneCommand** handler, handles the programmatic reorder part of the solution using Move method based on IsChecked property.

``` C#
public class ViewModel : INotifyPropertyChanged
{
    private ObservableCollection<ToDoItem> toDoList;
    private Command<object> markDoneCommand;

    public ViewModel()
    {
        MarkDoneCommand = new Command<object>(MarkItemAsDone);
    }

    public ObservableCollection<ToDoItem> ToDoList
    {
        get { return toDoList;  }
        set { this.toDoList = value; }
    }

    public Command<object> MarkDoneCommand
    {
        get
        {
            return markDoneCommand;
        }
        set
        {
            if (markDoneCommand != value)
            {
                markDoneCommand = value;
                OnPropertyChanged("MarkDoneCommand");
            }
        }
    }

    private void MarkItemAsDone(object obj)
    {
        var item = obj as ToDoItem;
        var currentindex = this.ToDoList.IndexOf(item);

        if (item.IsDone)
            this.ToDoList.Move(currentindex, this.ToDoList.Count - 1);
        else
            this.ToDoList.Move(currentindex, 0);
    }
}
```
**C#**

Returns the image based on IsChecked value.

``` c#
namespace ListViewXamarin
{
    public class ImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return (bool)value ? "Checked.png" : "UnChecked.png";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
```

**C#**

Use custom group converter to maintain the group order after reordering the items programmatically.

``` c#
namespace ListViewXamarin
{
    public class CustomGroupComparer : IComparer<GroupResult>
    {
        public int Compare(GroupResult x, GroupResult y)
        {
            if (x.Count > y.Count)
            {
                //GroupResult y is stacked into top of the group i.e., Ascending.
                //GroupResult x is stacked at the bottom of the group i.e., Descending.
                return 1;
            }
            else if (x.Count < y.Count)
            {
                //GroupResult x is stacked into top of the group i.e., Ascending.
                //GroupResult y is stacked at the bottom of the group i.e., Descending.
                return -1;
            }

            return 0;
        }
    }
}
```
**Output**

![ProgrammaticReOrder](https://github.com/SyncfusionExamples/programmatically-reorder-listview-xamarin/blob/master/ScreenShot/ProgrammaticReOrder.gif)
