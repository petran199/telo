select KRATISEIS.idKratisis as 'ID ΚΡΑΤΗΣΗΣ', 
       CUSTOMERS.idPelati as 'ID ΠΕΛΑΤΗ',
       CUSTOMERS.taftotita as 'ΤΑΥΤΟΤΗΤΑ ΠΕΛΑΤΗ',
       CUSTOMERS.epwnimo as 'ΕΠΩΝΥΜΟ',
       CUSTOMERS.onoma as 'ΟΝΟΜΑ',
       ROOMS.arithmosDwmatiou as 'ΑΡΙΘΜΟΣ ΔΩΜΑΤΙΟΥ',
       ROOMTYPE.perigrafi as 'ΤΥΠΟΣ ΔΩΜΑΤΙΟΥ',
      [GROUP].perigrafi as 'ΓΡΟΥΠ',
      KRATISEIS.hmerominiaAfixis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΦΙΞΗΣ',
      KRATISEIS.hmerominiaAnaxwrisis as 'ΗΜΕΡΟΜΗΝΙΑ ΑΝΑΧΩΡΗΣΗΣ',
      TyposKratisis.perigrafi as 'ΤΥΠΟΣ ΚΡΑΤΗΣΗΣ',
      tropoiPlirwmis.perigrafi as 'ΤΡΟΠΟΣ ΠΛΗΡΩΜΗΣ',
      exoflisiKratisis.perigrafi as 'ΕΞΟΦΛΗΣΗ ΚΡΑΤΗΣΗΣ'
 from KRATISEIS,ROOMS,ROOMTYPE,CUSTOMERS,[GROUP],tropoiPlirwmis,TyposKratisis, exoflisiKratisis
where ( KRATISEIS.idKratisis=(select idKratisis from KRATISEIS where idKratisis='" + txtbox_idKratisis_editKratisi.Text + "')
 or  KRATISEIS.idPelati=(select idPelati from CUSTOMERS where taftotita=N'" + txtbox_taftotitaPelati_editKratisi.Text + "'))
and KRATISEIS.idDwmatiou=ROOMS.idDwmatiou
  and KRATISEIS.idExoflisiKratisis=exoflisiKratisis.IdExoflisiKratisis
 and KRATISEIS.idGroup=[GROUP].idGroup and KRATISEIS.idPelati=CUSTOMERS.idPelati
  and KRATISEIS.idTroposPlirwmis=tropoiPlirwmis.idTroposPlirwmis
  and KRATISEIS.idTyposKratisis=TyposKratisis.idTyposKratisis
    and ROOMS.idTyposDwmatiou=ROOMTYPE.idTyposDwmatiou


	 ORDER BY hmerominiaAfixis DESC


	 KRATISEIS.idPelati=(select idPelati from CUSTOMERS where taftotita=N'ε')