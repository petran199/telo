insert into CUSTOMERS (idGroup,epwnimo,onoma,taftotita,afm,tilefwno,xwra,poli,odos,arithmos) 
 values((select idGroup from [GROUP] WHERE perigrafi=N'ΚΑΝΕΝΑ'),'A','A','1','1','1','A','A','A','1')

 SELECT * FROM CUSTOMERS

 insert into CUSTOMERS (idGroup,epwnimo,onoma,taftotita,afm,tilefwno,xwra,poli,odos,arithmos) 
 SELECT g.idGroup,c.epwnimo,c.onoma,c.taftotita,c.afm,c.tilefwno,c.xwra,c.poli,c.odos,c.arithmos
 from [GROUP] g,CUSTOMERS c
 where  g.perigrafi=N'ΚΑΝΕΝΑ'and c.epwnimo=N'b' and c.onoma=N'b' and c.taftotita=N'3'and c.afm=N'3'and c.tilefwno=N'3'and c.xwra=N'b'and c.poli=N'b' and 
 c.odos=N'b' and c.arithmos=N'3'

 select  idTyposDwmatiou from ROOMTYPE where perigrafi=N'ΜΟΝΟΚΛΙΝΟ'

 SELECT g.idGroup,c.epwnimo,c.onoma,c.taftotita,c.afm,c.tilefwno,c.xwra,c.poli,c.odos,c.arithmos
 from [GROUP] g,CUSTOMERS c


 insert into CUSTOMERS (idGroup,epwnimo,onoma,taftotita,afm,tilefwno,xwra,poli,odos,arithmos) 
 values((select idGroup from [GROUP] WHERE perigrafi=N'ΚΑΝΕΝΑ'),'A','A','1','1','1','A','A','A','1')