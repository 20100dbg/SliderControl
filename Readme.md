# SliderControl for .Net

## Introduction

Ce projet est configuré pour utilisé .Net framework 4.7.2.

Il est possible de configurer le projet pour utiliser une autre version


## Tester

La solution contient deux projets
- SliderControl : ce projet génère une DLL intégrable dans un projet externe.
- testSlider : ce projet permet de tester le control.

Pour tester le contrôle, sélectionner le projet testSlider, clic droit -> Déboguer -> Démarrer une nouvelle instance


## Compiler

Pour compiler, sélectionner le mode Release, puis menu Générer -> Générer la solution

La DLL est générée dans le dossier SliderControl\SliderControl\bin\Release\


## Intégrer dans un autre projet

Copier la DLL dans le projet destination

### Ajouter la DLL en référence du projet
	using SliderControl;

### Exemple d'initialisation
	Slider s = new Slider(this);

Le contrôle apparaît sur la Form.


## Détecter la modification de la valeur
	private void testSlider()
    {
        Slider s = new Slider(this);
        s.SpanMoved += S_SpanMoved;
    }
    private void S_SpanMoved(object sender, SpanMovedEventArgs e)
    {
        MessageBox.Show(e.NewValue.ToString());
    }


## Toutes les propriétés
	Slider s = new Slider(this);
	
	//Value (position du curseur)
	s.CurrentValue;
	s.SetValue(20);
	
	//Largeur du curseur
	s.CurrentSpan;
    s.SetSpan(20);
    
    //SmallChange : petit décalage (appui sur bouton)
    s.SmallChange = 5;

	//LargeChange : grand décalage (clic sur la barre de fond)
    s.LargeChange = 20;

    //Obtenir l'état du curseur (en redimensionnement, en mouvement, néant)
    s.SpanState


## Tous les évènements
	//Se déclenche pendant le redimensionnement du curseur
	s.SpanResizing += S_SpanResizing;
	
	//Se déclenche après le redimensionnement du curseur
	s.SpanResized += S_SpanResized;
	
	//Se déclenche pendant le déplacement du curseur
	s.SpanMoving += S_SpanMoving;
	
	//Se déclenche après le déplacement du curseur
	s.SpanMoved += S_SpanMoved;