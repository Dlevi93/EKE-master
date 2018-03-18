import { Component, OnInit, Inject } from '@angular/core';
import { Http } from '@angular/http';
import { CalendarModule } from 'primeng/calendar';
import { FormGroup, FormControl, FormBuilder, Validators } from '@angular/forms';

@Component({
    selector: 'app-registration',
    templateUrl: './registration.component.html',
    styleUrls: ['./registration.component.css']
})
/** registration component*/
export class RegistrationComponent implements OnInit {
    public myForm: FormGroup; // our model driven form
    public submitted: boolean; // keep track on whether form is submitted
    public events: any[] = []; // use later to display form changes
    url: Http;
    base: string;
    /** registration ctor */
    constructor(private _fb: FormBuilder, http: Http, @Inject('BASE_URL') baseUrl: string) {
        this.url = http;
        this.base = baseUrl;
    }

    ngOnInit() {
        // the short way
        this.myForm = this._fb.group({
            name: ['', [<any>Validators.required, <any>Validators.minLength(5)]],
            //address: this._fb.group({
            //    street: ['', <any>Validators.required],
            //    postcode: ['']
            //}),
            birthdate: ['', <any>Validators.required],
        });

        this.subcribeToFormChanges();
    }

    save(model: VtUser, isValid: boolean) {
        this.submitted = true; // set form submit to true
        // check if model is valid

        this.url.post(this.base + 'api/SampleData', model).subscribe(res => console.log(res.json()));
        // if valid, call API to save customer
        console.log(model, isValid);
    }

    subcribeToFormChanges() {
        // initialize stream
        const myFormValueChanges$ = this.myForm.valueChanges;

        // subscribe to the stream 
        myFormValueChanges$.subscribe(x => this.events
            .push({ event: 'STATUS CHANGED', object: x }));
    }
}

export interface VtUser {
    name: string;
    //address?: {
    //    street?: string; // required
    //    postcode?: string;
    //}
    birthdate: Date;
}