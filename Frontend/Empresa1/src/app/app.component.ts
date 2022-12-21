import { Component, OnInit, ViewChild } from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import { EmpresaComponent } from './empresa/empresa.component';
import { ApiService } from './services/api.service';
import {MatPaginator} from '@angular/material/paginator';
import {MatSort} from '@angular/material/sort';
import {MatTableDataSource} from '@angular/material/table';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  title = 'Empresa1';
  displayedColumns: string[] = ['id', 'estado', 'nombre_comercial', 'correo','nit', 'opciones'];
  dataSource !: MatTableDataSource<any>;

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  @ViewChild(MatSort) sort!: MatSort;

  constructor (private empresa : MatDialog, private api : ApiService){}
  ngOnInit(): void {
    this.getAllEmpresa()
  }
  openDialog() {
    const dialogRef = this.empresa.open(EmpresaComponent,{
      width:'70%'
    }).afterClosed().subscribe(val=>{
      if(val=='Agregar'){
        this.getAllEmpresa()
      }
    })

  }

  getAllEmpresa(){
    this.api.getEmpresas()
    .subscribe({
      next:(res)=>{
        console.log(res)
        this.dataSource = new MatTableDataSource(res)
        this.dataSource.paginator = this.paginator
        this.dataSource.sort=this.sort
      },
      error:(err)=>{
        console.log('Error inesperado')
      }
    })
  }

  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();

    if (this.dataSource.paginator) {
      this.dataSource.paginator.firstPage();
    }
  }

  editEmpresa(row:any){
    this.empresa.open(EmpresaComponent,{
      width:'70%',
      data:row
    }).afterClosed().subscribe(val=>{
      if(val=='Actualizar'){
        this.getAllEmpresa();
      }
    })
  }

  deleteEmpresa(id:number){
    this.api.deleteEmpres(id)
    .subscribe({
      next:(res)=>{
        alert("Empresa Eliminada Satisfactoriamente")
        this.getAllEmpresa()
      },
      error:()=>{
        alert("Error al intentar eliminar la empresa")
      }
    })
  }
}
