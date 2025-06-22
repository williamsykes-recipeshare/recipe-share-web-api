USE recipe_share;

-- ==========================================
-- INGREDIENTS
-- ==========================================
INSERT INTO mtn_ingredient (ingGuid, ingName, ingCreatedBy, ingCreatedByName, ingUpdatedBy, ingUpdatedByName)
VALUES
(UNHEX(REPLACE('a1111111-1111-1111-1111-111111111111','-','')), 'Chicken Breast', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a2222222-2222-2222-2222-222222222222','-','')), 'Garlic', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a3333333-3333-3333-3333-333333333333','-','')), 'Olive Oil', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a4444444-4444-4444-4444-444444444444','-','')), 'Tomato Sauce', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a5555555-5555-5555-5555-555555555555','-','')), 'Basil', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a6666666-6666-6666-6666-666666666666','-','')), 'Spaghetti', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a7777777-7777-7777-7777-777777777777','-','')), 'Ground Beef', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a8888888-8888-8888-8888-888888888888','-','')), 'Parmesan Cheese', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a9999999-9999-9999-9999-999999999999','-','')), 'Flour', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a1010101-1010-1010-1010-101010101010','-','')), 'Eggs', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a1212121-1212-1212-1212-121212121212','-','')), 'Butter', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a1313131-1313-1313-1313-131313131313','-','')), 'Sugar', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a1414141-1414-1414-1414-141414141414','-','')), 'Milk', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a1515151-1515-1515-1515-151515151515','-','')), 'Yogurt', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('a1616161-1616-1616-1616-161616161616','-','')), 'Honey', 1, 'William', 1, 'William');

-- ==========================================
-- RECIPES
-- ==========================================
INSERT INTO trx_recipe (rcpGuid, rcpName, rcpCookingTimeMinutes, rcpCreatedBy, rcpCreatedByName, rcpUpdatedBy, rcpUpdatedByName)
VALUES
(UNHEX(REPLACE('b1111111-1111-1111-1111-111111111111','-','')), 'Grilled Chicken Breast', 30, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('b2222222-2222-2222-2222-222222222222','-','')), 'Spaghetti Bolognese', 45, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('b3333333-3333-3333-3333-333333333333','-','')), 'Tomato Basil Pasta', 25, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('b4444444-4444-4444-4444-444444444444','-','')), 'Beef Lasagna', 60, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('b5555555-5555-5555-5555-555555555555','-','')), 'Pancakes', 20, 1, 'William', 1, 'William');

-- ==========================================
-- STEPS
-- ==========================================
INSERT INTO mtn_step (stpGuid, stpIndex, stpName, stpRecipeId, stpCreatedBy, stpCreatedByName, stpUpdatedBy, stpUpdatedByName)
VALUES
-- Grilled Chicken Breast (5 steps)
(UNHEX(REPLACE('c1111111-1111-1111-1111-111111111111','-','')), 1, 'Season chicken', 1, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c1111112-1111-1111-1111-111111111112','-','')), 2, 'Heat grill', 1, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c1111113-1111-1111-1111-111111111113','-','')), 3, 'Grill chicken 6 min each side', 1, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c1111114-1111-1111-1111-111111111114','-','')), 4, 'Check doneness', 1, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c1111115-1111-1111-1111-111111111115','-','')), 5, 'Serve with sauce', 1, 1, 'William', 1, 'William'),

-- Spaghetti Bolognese (6 steps)
(UNHEX(REPLACE('c2222221-2222-2222-2222-222222222221','-','')), 1, 'Cook spaghetti', 2, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c2222222-2222-2222-2222-222222222222','-','')), 2, 'Heat oil', 2, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c2222223-2222-2222-2222-222222222223','-','')), 3, 'Cook beef', 2, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c2222224-2222-2222-2222-222222222224','-','')), 4, 'Add tomato sauce', 2, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c2222225-2222-2222-2222-222222222225','-','')), 5, 'Simmer sauce', 2, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c2222226-2222-2222-2222-222222222226','-','')), 6, 'Serve with cheese', 2, 1, 'William', 1, 'William'),

-- Tomato Basil Pasta (5 steps)
(UNHEX(REPLACE('c3333331-3333-3333-3333-333333333331','-','')), 1, 'Boil pasta', 3, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c3333332-3333-3333-3333-333333333332','-','')), 2, 'Make sauce', 3, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c3333333-3333-3333-3333-333333333333','-','')), 3, 'Add basil', 3, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c3333334-3333-3333-3333-333333333334','-','')), 4, 'Combine pasta & sauce', 3, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c3333335-3333-3333-3333-333333333335','-','')), 5, 'Serve hot', 3, 1, 'William', 1, 'William'),

-- Beef Lasagna (7 steps)
(UNHEX(REPLACE('c4444441-4444-4444-4444-444444444441','-','')), 1, 'Prepare beef sauce', 4, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c4444442-4444-4444-4444-444444444442','-','')), 2, 'Make béchamel', 4, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c4444443-4444-4444-4444-444444444443','-','')), 3, 'Layer pasta & sauce', 4, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c4444444-4444-4444-4444-444444444444','-','')), 4, 'Add cheese', 4, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c4444445-4444-4444-4444-444444444445','-','')), 5, 'Repeat layers', 4, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c4444446-4444-4444-4444-444444444446','-','')), 6, 'Bake', 4, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c4444447-4444-4444-4444-444444444447','-','')), 7, 'Cool & serve', 4, 1, 'William', 1, 'William'),

-- Pancakes (5 steps)
(UNHEX(REPLACE('c5555551-5555-5555-5555-555555555551','-','')), 1, 'Mix flour, milk, eggs', 5, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c5555552-5555-5555-5555-555555555552','-','')), 2, 'Heat pan', 5, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c5555553-5555-5555-5555-555555555553','-','')), 3, 'Pour batter', 5, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c5555554-5555-5555-5555-555555555554','-','')), 4, 'Flip pancake', 5, 1, 'William', 1, 'William'),
(UNHEX(REPLACE('c5555555-5555-5555-5555-555555555555','-','')), 5, 'Serve with honey', 5, 1, 'William', 1, 'William');

-- ==========================================
-- DIETARY TAGS
-- ==========================================
INSERT INTO mtn_dietary_tag (dtgGuid, dtgName, dtgCreatedBy, dtgCreatedByName, dtgUpdatedBy, dtgUpdatedByName)
VALUES
(UNHEX(REPLACE('d1111111-1111-1111-1111-111111111111','-','')), 'High Protein', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('d2222222-2222-2222-2222-222222222222','-','')), 'Quick Meal', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('d3333333-3333-3333-3333-333333333333','-','')), 'Comfort Food', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('d4444444-4444-4444-4444-444444444444','-','')), 'Family Favorite', 1, 'William', 1, 'William'),
(UNHEX(REPLACE('d5555555-5555-5555-5555-555555555555','-','')), 'Kid Friendly', 1, 'William', 1, 'William');

-- ==========================================
-- LINK RECIPES TO DIETARY TAGS
-- ==========================================
INSERT INTO mtn_recipe_dietary_tag (rdtRecipeId, rdtDietaryTagId, rdtCreatedBy, rdtCreatedByName, rdtUpdatedBy, rdtUpdatedByName)
VALUES
(1, 1, 1, 'William', 1, 'William'),
(1, 2, 1, 'William', 1, 'William'),
(2, 1, 1, 'William', 1, 'William'),
(2, 3, 1, 'William', 1, 'William'),
(3, 2, 1, 'William', 1, 'William'),
(3, 3, 1, 'William', 1, 'William'),
(3, 4, 1, 'William', 1, 'William'),
(4, 3, 1, 'William', 1, 'William'),
(4, 4, 1, 'William', 1, 'William'),
(5, 2, 1, 'William', 1, 'William'),
(5, 5, 1, 'William', 1, 'William');

-- ==========================================
-- LINK RECIPES TO INGREDIENTS
-- ==========================================
INSERT INTO mtn_recipe_ingredient (rpiRecipeId, rpiIngredientId, rpiQuantity, rpiCreatedBy, rpiCreatedByName, rpiUpdatedBy, rpiUpdatedByName)
VALUES
-- Grilled Chicken Breast
(1, 1, '200g', 1, 'William', 1, 'William'),
(1, 2, '2 cloves', 1, 'William', 1, 'William'),
(1, 3, '1 tbsp', 1, 'William', 1, 'William'),

-- Spaghetti Bolognese
(2, 6, '200g', 1, 'William', 1, 'William'),
(2, 4, '150g', 1, 'William', 1, 'William'),
(2, 7, '250g', 1, 'William', 1, 'William'),
(2, 3, '2 tbsp', 1, 'William', 1, 'William'),

-- Tomato Basil Pasta
(3, 6, '200g', 1, 'William', 1, 'William'),
(3, 4, '100g', 1, 'William', 1, 'William'),
(3, 5, '10 leaves', 1, 'William', 1, 'William'),

-- Beef Lasagna
(4, 7, '300g', 1, 'William', 1, 'William'),
(4, 4, '200g', 1, 'William', 1, 'William'),
(4, 9, '100g', 1, 'William', 1, 'William'),
(4, 8, '100g', 1, 'William', 1, 'William'),

-- Pancakes
(5, 9, '150g', 1, 'William', 1, 'William'),
(5, 10, '2 pieces', 1, 'William', 1, 'William'),
(5, 11, '50g', 1, 'William', 1, 'William'),
(5, 13, '2 tbsp', 1, 'William', 1, 'William');