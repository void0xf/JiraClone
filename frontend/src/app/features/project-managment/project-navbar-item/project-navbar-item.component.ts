import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgIcon, provideIcons } from '@ng-icons/core';
import { Router } from '@angular/router'; // Import Router
import { ActivatedRoute } from '@angular/router';

import * as lucideIcons from '@ng-icons/lucide';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-project-navbar-item',
  imports: [NgIcon],
  providers: [
    provideIcons({
      ...lucideIcons,
    }),
  ],
  templateUrl: './project-navbar-item.component.html',
  styleUrl: './project-navbar-item.component.scss',
})
export class ProjectNavbarItemComponent implements OnInit, OnDestroy {
  projectKey: string | null = null;
  private routeSubscription: Subscription | undefined; // To manage subscription
  @Input({ required: true }) name: string = '';
  @Input() lucideIconName: keyof typeof lucideIcons | null = null;

  constructor(private router: Router, private route: ActivatedRoute) {}
  ngOnInit(): void {
    this.routeSubscription = this.route.paramMap.subscribe((params) => {
      this.projectKey = params.get('project_key');
    });
  }
  ngOnDestroy(): void {
    if (this.routeSubscription) {
      this.routeSubscription.unsubscribe();
    }
  }

  handleProjectNavigation() {
    const categorySegment = this.name.toLowerCase();

    this.router.navigate([
      'jira',
      'software',
      'projects',
      this.projectKey,
      categorySegment,
    ]);
  }
}
