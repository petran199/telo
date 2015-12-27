select r.arithmosDwmatiou, rt.perigrafi, tk.perigrafi, datediff(day,kr.hmerominiaAfixis, kr.hmerominiaAnaxwrisis) daysdiff
  from ROOMS r 
  left join ROOMTYPE rt
         on r.idDwmatiou = rt.idTyposDwmatiou
  left join KRATISEIS kr
         on kr.idDwmatiou = r.idDwmatiou
  left join TyposKratisis tk
         on tk.idTyposKratisis = kr.idTyposKratisis
