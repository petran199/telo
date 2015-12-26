select idGroup from [GROUP] where perigrafi=N'ΚΑΝΕΝΑ'

"insert into CUSTOMERS (idGroup,epwnimo,onoma,taftotita,afm,tilefwno,xwra,poli,odos,arithmos)   values((select idGroup from [GROUP] WHERE perigrafi= N'ΓΚΡΟΥΠ 1 (ΠΑΡΑΔΟΣΙΑΚΟΣ)'),N'y',N'y',N'',N'6',N'6',N'y',N'y',N'y',N'6')"

insert into CUSTOMERS (idGroup,epwnimo,onoma,taftotita,afm,tilefwno,xwra,poli,odos,arithmos)   values('4''" + txtbox_lname_add_customer.Text + "',N'" + txtbox_fname_add_customer.Text + "',N'" + txtbox_taftotita_add_customer.Text + "',N'" + txtbox_afm_add_customer.Text + "',N'" + txtbox_tel_add_customer.Text + "',N'" + txtbox_country_add_customer.Text + "',N'" + txtbox_city_add_customer.Text + "',N'" + txtbox_odos_add_customer.Text + "',N'" + txtbox_arithmosodou_add_customer.Text + "')