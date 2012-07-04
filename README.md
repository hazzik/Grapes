Grapes
======

Grapes it is an utility library which takes care of mapping TreeEntry into database

#How to use?

##Installation
First of all install adapter for your favorite ORM (#NHibernate and #EntityFramework CodeFirst are supported):

    Install-Package Brandy.Grapes.NHibernate
    
OR

    Install-Package Brandy.Grapes.FluentNHibernate
    
##Usage

###Inherit your class from TreeEntry`1

```csharp
public class MySuperTree : TreeEntry<MySuperTree>
{
    public virtual int Id { get; set; }

    public virtual string Name { get; set; }
}
```

###Write a mapping with your favarite ORM:

####FluentNhibernate:

```csharp
using Brandy.Grapes.FluentNHibernate;
public class MySuperTreeMap : ClassMap<MySuperTree>
{
    public MySuperTreeMap()
    {
        Id(x => x.Id);
        Map(x => x.Name);
        
        this.MapTree("MySuperTreeHierarchy"); // all magic goes here
    }
}
```

###Enjoy