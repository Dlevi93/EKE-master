import { Component, OnInit, Inject, Input } from '@angular/core';
import { Http } from '@angular/http';
import { CalendarModule } from 'primeng/calendar';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';
import { NavbarComponent } from '../registrationflow/navbar/navbar.component';
import { FormDataService } from '../registrationflow/data/formData.service';

@Component({
    selector: 'app-registration',
    templateUrl: './registration.component.html',
    styleUrls: ['./registration.component.css']
})

/** registration component*/
export class RegistrationComponent implements OnInit {
    title = 'Multi-Step Wizard';
    @Input() formData: any;

    constructor(private formDataService: FormDataService) {
    }

    ngOnInit() {
        this.formData = this.formDataService.getFormData();
        console.log(this.title + ' loaded!');
    }
}