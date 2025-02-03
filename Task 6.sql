if not exists (select * from sys.tables where name = 'teacher')
create table teacher (
    teacherid int primary key identity(1,1),
    firstname nvarchar(50) not null,
    lastname nvarchar(50) not null,
    gender nvarchar(10),
    subject nvarchar(100)
);

if not exists (select * from sys.tables where name = 'pupil')
create table pupil (
    pupilid int primary key identity(1,1),
    firstname nvarchar(50) not null,
    lastname nvarchar(50) not null,
    gender nvarchar(10),
    class nvarchar(20)
);

if not exists (select * from sys.tables where name = 'teacherpupil')
create table teacherpupil (
    teacherid int not null,
    pupilid int not null,
    primary key (teacherid, pupilid),
    foreign key (teacherid) references teacher(teacherid),
    foreign key (pupilid) references pupil(pupilid)
);

insert into teacher (firstname, lastname, gender, subject)
select N'ნინო', N'ყურაშვილი', N'მდედრობითი', N'მათემატიკა'
where not exists (
    select 1 from teacher 
    where firstname = N'ნინო' and lastname = N'ყურაშვილი'
);

insert into pupil (firstname, lastname, gender, class)
select N'გიორგი', N'მამულაშვილი', N'მამრობითი', N'10-ა'
where not exists (
    select 1 from pupil 
    where firstname = N'გიორგი' and lastname = N'მამულაშვილი'
);

insert into teacherpupil (teacherid, pupilid)
select t.teacherid, p.pupilid
from teacher t
cross join pupil p
where t.firstname = N'ნინო'
  and p.firstname = N'გიორგი'
  and not exists (
    select 1 from teacherpupil
    where teacherid = t.teacherid and pupilid = p.pupilid
  );

select t.teacherid,
       t.firstname as teacherfirstname,
       t.lastname as teacherlastname,
       t.subject
from teacher t
inner join teacherpupil tp on t.teacherid = tp.teacherid
inner join pupil p on tp.pupilid = p.pupilid
where p.firstname = N'გიორგი';


