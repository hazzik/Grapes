Grapes
======

Grapes it is an utility library which takes care of mapping TreeEntry into database

#How to use?

##Installation
First of all install adapter for your favorite ORM (#NHibernate and #EntityFramework CodeFirst are supported):

    Install-Package Brandy.Grapes.EntityFramework
    
OR

    Install-Package Brandy.Grapes.NHibernate
    
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

####EntityFramework Code First:

```csharp

using Brandy.Grapes.EntityFramework;
public class MySuperTreeMap : EntityTypeConfiguration<MySuperTree>
{
    public MySuperTreeMap()
    {
        HasKey(x => x.Id);
        Property(x => x.Name);
        this.MapTree("MySuperTreeHierarchy");
    }
}

```

####FluentNhibernate:

```csharp
using Brandy.Grapes.NHibernate;
public class MySuperTreeMap : ClassMap<MySuperTree>
{
    public MySuperTreeMap()
    {
        Id(x => x.Id);
        Map(x => x.Name);
        this.MapTree("MySuperTreeHierarchy");
    }
}
```

###Enjoy