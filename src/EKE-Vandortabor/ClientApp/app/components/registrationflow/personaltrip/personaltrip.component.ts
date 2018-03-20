import { Component, OnInit, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http'; 
import { FormDataService } from '../data/formData.service';

@Component({
    selector: 'mt-wizard-personaltrip',
    templateUrl: './personaltrip.component.html',
    styleUrls: ['./personaltrip.component.css']
})

export class PersonalTripComponent implements OnInit {
    memberships: Membership[] = [];
    accomodationTypes: AccomodationType[];

    selectedValueMembership: Membership;
    selectedAccomodationTypes: AccomodationType;

    workType: string;
    form: any;

    constructor(private router: Router, private formDataService: FormDataService, http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
        http.get(baseUrl + 'api/SampleData/Memberships').subscribe(result => {
            this.memberships = result as Membership[];
        }, error => console.error(error));
        http.get(baseUrl + 'api/SampleData/AccomodationTypes').subscribe(result => {
            this.accomodationTypes = result as AccomodationType[];
        }, error => console.error(error));
    }

    ngOnInit() {
        this.workType = this.formDataService.getWork();
        console.log('Personal trip feature loaded!');
    }

    save(form: any): boolean {
        if (!form.valid) {
            return false;
        }

        this.formDataService.setWork(this.workType);
        return true;
    }

    goToPrevious(form: any) {
        if (this.save(form)) {
            // Navigate to the personal page
            this.router.navigate(['registration/personal']);
        }
    }

    goToNext(form: any) {
        if (this.save(form)) {
            // Navigate to the address page
            this.router.navigate(['registration/address']);
        }
    }
}

interface Membership {
    id: number;
    name: string;
    enum: number;
}

interface AccomodationType {
    id: number;
    name: string;
    enum: number;
}