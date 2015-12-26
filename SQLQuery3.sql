select idDwmatiou,arithmosDwmatiou
 from ROOMS 
 where idTyposDwmatiou = 1
  and idDwmatiou NOT IN (
  select idDwmatiou 
 from KRATISEIS
 where hmerominiaAfixis <= '2015/12/8' and hmerominiaAnaxwrisis >'2015/12/7')
 order by arithmosDwmatiou