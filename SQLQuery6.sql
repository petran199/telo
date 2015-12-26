select idDwmatiou,arithmosDwmatiou 
  from ROOMS 
 where idTyposDwmatiou = 2
   and idDwmatiou NOT IN (select idDwmatiou 
                            from KRATISEIS
						    where (hmerominiaAfixis <= '2015-12-05' and hmerominiaAnaxwrisis > '2015-12-05') OR
							      (hmerominiaAfixis< '2015-12-08' AND hmerominiaAnaxwrisis>= '2015-12-08') OR
								  (hmerominiaAfixis>='2015-12-05' and hmerominiaAfixis<= '2015-12-08'))
	or (idDwmatiou = '25' and idTyposDwmatiou = 2);




	"select idDwmatiou,arithmosDwmatiou from ROOMS where idTyposDwmatiou =  and idDwmatiou NOT IN (select idDwmatiou from KRATISEIS where (hmerominiaAfixis <= @checkinDate and hmerominiaAnaxwrisis > @checkinDate) OR (hmerominiaAfixis<@checkoutte AND hmerominiaAnaxwrisis>=@checkoutte) OR (hmerominiaAfixis>=@checkinDate and hmerominiaAfixis<=@checkoutte)) OR ( idDwmatiou = '' and idTyposDwmatiou = '')"


