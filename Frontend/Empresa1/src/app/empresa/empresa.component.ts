import { Component, Inject } from '@angular/core';
import { FormGroup,FormBuilder, Validators } from '@angular/forms';
import { ApiService } from '../services/api.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-empresa',
  templateUrl: './empresa.component.html',
  styleUrls: ['./empresa.component.scss']
})
export class EmpresaComponent {
  empresaForm !: FormGroup
  titulo1 : string = "Agregar Empresa"
  titulo2 : string = "Ingresa los datos"
  tipobtn : string = "Agregar"
  editar : boolean = false
  constructor(private formBuilder : FormBuilder , private api : ApiService, @Inject(MAT_DIALOG_DATA) public editData:any, private dialogRef : MatDialogRef<EmpresaComponent>){}

  ngOnInit() : void{
    this.empresaForm = this.formBuilder.group({
      nombre_comercial : ['',Validators.required],
      razon_social :['',Validators.required],
      telefono :['',Validators.required],
      correo :['',Validators.required],
      nit :['',Validators.required],
      estado :['',Validators.required],
      direccion :['',Validators.required]
    })

    if(this.editData){
      this.titulo1="Editar Empresa"
      this.titulo2="Actualiza los campos"
      this.tipobtn="Actualizar"
      this.editar=true
      this.empresaForm.controls['nombre_comercial'].setValue(this.editData.nombre_comercial)
      this.empresaForm.controls['razon_social'].setValue(this.editData.razon_social)
      this.empresaForm.controls['telefono'].setValue(this.editData.telefono)
      this.empresaForm.controls['correo'].setValue(this.editData.correo)
      this.empresaForm.controls['nit'].setValue(this.editData.nit)
      this.empresaForm.controls['estado'].setValue(this.editData.estado)
      this.empresaForm.controls['direccion'].setValue(this.editData.direccion)
    }
  }

  addEmpresa(){
    if(!this.editData){
      if(this.empresaForm.valid){
        this.api.postEmpresa(this.empresaForm.value)
        .subscribe({
          next:(res) =>{
            alert("add empresa")
            this.empresaForm.reset()
            this.dialogRef.close('Agregar')
          },
          error:()=>{
            alert('ERROR NO FUE POSIBLE')
          }
        })
      }
    }else{
      this.updateEmpresa()
    }
    
  }

  updateEmpresa(){
    this.api.putEmpresa(this.empresaForm.value,this.editData.id)
    .subscribe({
      next:(res)=>{
        alert("actualizacion exitosa");
        this.empresaForm.reset()
        this.dialogRef.close('Actualizar')
      },
      error:()=>{
        alert("Error durante la actualizacion")
      }
    })
  }

}
